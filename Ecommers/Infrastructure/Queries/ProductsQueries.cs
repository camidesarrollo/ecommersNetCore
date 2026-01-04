using Ecommers.Application.DTOs.Common;
using Ecommers.Infrastructure.Persistence;

namespace Ecommers.Infrastructure.Queries
{
    public class ProductsQueries
    {
        public static int GetCountByCategories(long Id)
        {
            using var db = new EcommersContext();

            var product = db.Products.Where(x => x.CategoryId == Id).Count();

            return product;
        }
        public static int GetCountByMasterAttributes(long id)
        {
            using var db = new EcommersContext();

            var count = db.Products
                .Count(p => p.ProductAttributes.Any(a =>
                            a.Value.AttributeId == id
                        )
                );

            return count;
        }




    }
}
