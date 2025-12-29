using System.Linq.Expressions;

namespace Ecommers.Application.Common.Query
{
    public static class IQueryableSearchExtensions
    {
        public static IQueryable<T> ApplySearchFilter<T>(
            this IQueryable<T> query,
            string? term)
        {
            if (string.IsNullOrWhiteSpace(term))
                return query;

            term = term.Trim().ToLowerInvariant();

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression? finalExpression = null;

            var toLower = typeof(string).GetMethod("ToLower", Type.EmptyTypes)!;
            var contains = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;

            foreach (var prop in typeof(T).GetProperties())
            {
                Expression? exp = null;

                // === STRING ===
                if (prop.PropertyType == typeof(string))
                {
                    var access = Expression.Property(parameter, prop);
                    var notNull = Expression.NotEqual(access, Expression.Constant(null));
                    var lower = Expression.Call(access, toLower);
                    var check = Expression.Call(lower, contains, Expression.Constant(term));

                    exp = Expression.AndAlso(notNull, check);
                }

                // === INT ===
                else if (prop.PropertyType == typeof(int)
                      || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(int))
                {
                    var access = Expression.Property(parameter, prop);
                    var toString = Expression.Call(access, typeof(object).GetMethod("ToString")!);
                    var lower = Expression.Call(toString, toLower);
                    exp = Expression.Call(lower, contains, Expression.Constant(term));
                }

                // === DATE ===
                else if (prop.PropertyType == typeof(DateTime)
                      || Nullable.GetUnderlyingType(prop.PropertyType) == typeof(DateTime))
                {
                    if (DateTime.TryParse(term, out var date))
                    {
                        Expression access = Expression.Property(parameter, prop);

                        if (Nullable.GetUnderlyingType(prop.PropertyType) != null)
                        {
                            var hasValue = Expression.Property(access, "HasValue");
                            var value = Expression.Property(access, "Value");
                            var dateProp = Expression.Property(value, nameof(DateTime.Date));
                            var equals = Expression.Equal(dateProp, Expression.Constant(date.Date));
                            exp = Expression.AndAlso(hasValue, equals);
                        }
                        else
                        {
                            var dateProp = Expression.Property(access, nameof(DateTime.Date));
                            exp = Expression.Equal(dateProp, Expression.Constant(date.Date));
                        }
                    }
                }

                // === COLECCIONES ===
                else if (typeof(System.Collections.IEnumerable).IsAssignableFrom(prop.PropertyType)
                      && prop.PropertyType != typeof(string))
                {
                    var elementType = prop.PropertyType.IsGenericType
                        ? prop.PropertyType.GetGenericArguments()[0]
                        : prop.PropertyType.GetElementType();

                    if (elementType == null)
                        continue;

                    var stringProps = elementType.GetProperties()
                        .Where(p => p.PropertyType == typeof(string))
                        .ToList();

                    if (!stringProps.Any())
                        continue;

                    var collection = Expression.Property(parameter, prop);

                    var firstMethod = typeof(Enumerable)
                        .GetMethods()
                        .First(m => m.Name == "FirstOrDefault" && m.GetParameters().Length == 1)
                        .MakeGenericMethod(elementType);

                    var first = Expression.Call(firstMethod, collection);

                    Expression? inner = null;

                    foreach (var sp in stringProps)
                    {
                        var access = Expression.Property(first, sp);
                        var notNull = Expression.NotEqual(access, Expression.Constant(null));
                        var lower = Expression.Call(access, toLower);
                        var check = Expression.Call(lower, contains, Expression.Constant(term));

                        var condition = Expression.AndAlso(notNull, check);
                        inner = inner == null ? condition : Expression.OrElse(inner, condition);
                    }

                    exp = inner;
                }

                if (exp != null)
                    finalExpression = finalExpression == null
                        ? exp
                        : Expression.OrElse(finalExpression, exp);
            }

            if (finalExpression == null)
                return query;

            var lambda = Expression.Lambda<Func<T, bool>>(finalExpression, parameter);
            return query.Where(lambda);
        }
    }
}
