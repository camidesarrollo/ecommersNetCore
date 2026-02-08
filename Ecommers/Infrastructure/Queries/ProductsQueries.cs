using Ecommers.Application.DTOs.Common;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Infrastructure.Queries
{
    public class ProductsQueries : CommonQueries<Products>
    {
        public static int GetCountByCategories(EcommersContext context, long Id)
        {

            var product = context.Products.Where(x => x.CategoryId == Id).Count();

            return product;
        }
        public static int GetCountByMasterAttributes(EcommersContext context, long id)
        {

            var count = context.Products
                .Count(p => p.ProductAttributes.Any(a =>
                            a.Value.AttributeId == id
                        )
                );

            return count;
        }

        public static Products? GetProductsById(EcommersContext context, long productId)
        {
            var product = context.Products
                .AsNoTracking()
                .AsSplitQuery() // Divide en múltiples queries SQL
                .Include(p => p.ProductImages)
                .Include(p => p.ProductAttributes)
                    .ThenInclude(va => va.Value)
                .Include(p => p.ProductAttributes)
                    .ThenInclude(va => va.Attribute)
                .Include(p => p.Category)
                .Include(p => p.ProductVariants)
                    .ThenInclude(v => v.ProductVariantImages)
                .Include(p => p.ProductVariants)
                    .ThenInclude(v => v.VariantAttributes)
                        .ThenInclude(va => va.Attribute)
                .Include(p => p.ProductVariants)
                    .ThenInclude(v => v.VariantAttributes)
                        .ThenInclude(va => va.Value)
                .FirstOrDefault(p => p.Id == productId);

            return product;
        }
    }
}
