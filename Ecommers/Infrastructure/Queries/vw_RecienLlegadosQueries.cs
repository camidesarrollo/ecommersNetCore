using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Infrastructure.Queries
{
    public class vw_RecienLlegadosQueries
    {
        /*-- Todos los recién llegados ordenados por fecha*/
        public static List<vw_RecienLlegados> Get(EcommersContext context)
        {
            return [.. context.vw_RecienLlegados.OrderByDescending(x => x.FechaCreacion)];
        }

        /*-- Top 10 recién llegados*/
        public static List<vw_RecienLlegados> GetTop10(EcommersContext context)
        {
            return [.. context.vw_RecienLlegados.OrderByDescending(x => x.FechaCreacion).Take(10)];
        }

        /*-- Recién llegados de una categoría específica*/
        public static List<vw_RecienLlegados> GetByCategory(EcommersContext context, long categoryId)
        {
            return [.. context.vw_RecienLlegados
                .Where(x => x.CategoryId == categoryId)
                .OrderByDescending(x => x.FechaCreacion)];
        }

        /*-- Recién llegados con descuento*/
        public static List<vw_RecienLlegados> GetWithDiscount(EcommersContext context)
        {
            return [.. context.vw_RecienLlegados
                .Where(x => x.TieneDescuento == 1)
                .OrderByDescending(x => x.FechaCreacion)];
        }

        /*-- Recién llegados disponibles en stock*/
        public static List<vw_RecienLlegados> GetInStock(EcommersContext context)
        {
            return [.. context.vw_RecienLlegados
                .Where(x => x.StockStatus == "in_stock")
                .OrderByDescending(x => x.FechaCreacion)];
        }

        /*-- Recién llegados por rango de precio*/
        public static List<vw_RecienLlegados> GetByPriceRange(EcommersContext context, decimal priceStart, decimal priceEnd)
        {
            return [.. context.vw_RecienLlegados.Where(x => x.Price >= priceStart && x.Price <= priceEnd)];
        }

        /*-- -- Últimos 7 días (muy recientes)*/
        public static List<vw_RecienLlegados> GetLast7Days(EcommersContext context)
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
            return [.. context.vw_RecienLlegados.Where(x => x.FechaCreacion >= sevenDaysAgo).OrderByDescending(x => x.FechaCreacion)];
        }

        /*Últimos 15 días (recientes)*/
        public static List<vw_RecienLlegados> GetLast15Days(EcommersContext context)
        {
            var fifteenDaysAgo = DateTime.UtcNow.AddDays(-15);
            return [.. context.vw_RecienLlegados.Where(x => x.FechaCreacion >= fifteenDaysAgo).OrderByDescending(x => x.FechaCreacion)];
        }

        /*Últimos 30 días (recién llegados)*/
        public static List<vw_RecienLlegados> GetLast30Days(EcommersContext context)
        {
            var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
            return [.. context.vw_RecienLlegados.Where(x => x.FechaCreacion >= thirtyDaysAgo).OrderByDescending(x => x.FechaCreacion)];
        }
    }
}
