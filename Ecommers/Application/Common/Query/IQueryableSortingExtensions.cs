using System.Linq.Expressions;
using System.Reflection;

namespace Ecommers.Application.Common.Query
{

    public static class IQueryableSortingExtensions
    {
        public static IQueryable<T> ApplySorting<T>(
            this IQueryable<T> query,
            string? sortBy,
            bool ascending = true)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression propertyAccess = parameter;

            foreach (var part in sortBy.Split('.'))
            {
                var cleaned = part.Replace("[]", "");

                var property = propertyAccess.Type
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .FirstOrDefault(p =>
                        string.Equals(p.Name, cleaned, StringComparison.OrdinalIgnoreCase));

                if (property == null)
                    throw new ArgumentException($"Property '{cleaned}' not found on '{propertyAccess.Type.Name}'");

                propertyAccess = Expression.Property(propertyAccess, property);

                if (part.EndsWith("[]"))
                {
                    var elementType = property.PropertyType.GetGenericArguments().FirstOrDefault()
                                      ?? property.PropertyType.GetElementType()
                                      ?? throw new Exception($"'{property.Name}' is not a valid collection");

                    var firstMethod = typeof(Enumerable)
                        .GetMethods()
                        .First(m => m.Name == "FirstOrDefault" && m.GetParameters().Length == 1)
                        .MakeGenericMethod(elementType);

                    propertyAccess = Expression.Call(firstMethod, propertyAccess);
                }
            }

            var keySelector = Expression.Lambda(propertyAccess, parameter);
            var methodName = ascending ? "OrderBy" : "OrderByDescending";

            var method = typeof(Queryable).GetMethods()
                .First(m => m.Name == methodName && m.GetParameters().Length == 2)
                .MakeGenericMethod(typeof(T), propertyAccess.Type);

            return (IQueryable<T>)method.Invoke(null, new object[] { query, keySelector })!;
        }
    }
}
