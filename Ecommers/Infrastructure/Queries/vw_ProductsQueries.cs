using System.Linq.Expressions;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Infrastructure.Queries
{
    public class vw_ProductsQueries
    {
        // ✅ CORREGIDO: Retorna IQueryable en lugar de List
        public static IQueryable<vw_Products> GetByVW_Products(EcommersContext context)
        {
            // Retornar el IQueryable directamente sin materializarlo
            return context.vw_Products.AsNoTracking();
        }

        // Método de búsqueda optimizado
        public static IQueryable<vw_Products> ApplySearchFilter(
            IQueryable<vw_Products> query,
            string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
                return query;

            searchValue = searchValue.ToLower();

            return query.Where(p =>
                p.ProductName.ToLower().Contains(searchValue) ||
                p.CategoryName.ToLower().Contains(searchValue) ||
                (p.Brand != null && p.Brand.ToLower().Contains(searchValue)) ||
                (p.DefaultSKU != null && p.DefaultSKU.ToLower().Contains(searchValue)) ||
                (p.VariantSKUs != null && p.VariantSKUs.ToLower().Contains(searchValue)) ||
                (p.ShortDescription != null && p.ShortDescription.ToLower().Contains(searchValue))
            );
        }

        // Método de ordenamiento dinámico
        public static IQueryable<vw_Products> ApplySorting(
            IQueryable<vw_Products> query,
            string sortBy,
            bool ascending)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return query.OrderByDescending(p => p.CreatedAt);

            // Diccionario de expresiones de ordenamiento
            var sortExpressions = new Dictionary<string, Expression<Func<vw_Products, object>>>
            {
                { "productId", p => p.ProductId },
                { "productName", p => p.ProductName },
                { "categoryName", p => p.CategoryName },
                { "brand", p => p.Brand ?? "" },
                { "defaultSKU", p => p.DefaultSKU ?? "" },
                { "minVariantPrice", p => p.MinVariantPrice ?? 0 },
                { "totalStock", p => p.TotalStock },
                { "totalVariants", p => p.TotalVariants },
                { "isActive", p => p.IsActive },
                { "createdAt", p => p.CreatedAt },
                { "stockStatus", p => p.StockStatus ?? "" }
            };

            var sortKey = sortBy.ToLower();

            if (sortExpressions.TryGetValue(sortKey, out var expression))
            {
                return ascending
                    ? query.OrderBy(expression)
                    : query.OrderByDescending(expression);
            }

            // Default sort
            return query.OrderByDescending(p => p.CreatedAt);
        }
    }
}
