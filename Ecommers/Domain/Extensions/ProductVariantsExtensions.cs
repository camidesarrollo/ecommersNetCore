using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Domain.Extensions
{
    public class ProductVariantsExtensions
    {
        public static (long stockQuantity, string stockStatus, bool manageStock) DeterminarStock(ProductVariantsD pv)
        {
            if (pv.StockQuantity <= 0)
                return (0, "out_of_stock", true);

            if (pv.StockQuantity <= 10)
                return (pv.StockQuantity, "low_stock", true);

            return (pv.StockQuantity, "in_stock", true);
        }
    }
}
