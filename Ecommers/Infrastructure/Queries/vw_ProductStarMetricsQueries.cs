using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Infrastructure.Queries
{
    public class vw_ProductStarMetricsQueries
    {
        /* Top 10 productos estrella */

        public static List<vw_ProductStarMetrics> GetTop10(EcommersContext context)
        {
            return [.. context.vw_ProductStarMetrics
                .OrderByDescending(p => p.StarScore)
                .ThenByDescending(p => p.Sales_90_Days)
                .Take(10)];
        }

        /* -- Productos Gold Star */
        public static List<vw_ProductStarMetrics> GetGoldStarProducts(EcommersContext context)
        {
            return [.. context.vw_ProductStarMetrics
                .Where(p => p.StarLevel == "Gold Star")
                .OrderByDescending(p => p.StarScore)
                .ThenByDescending(p => p.Sales_90_Days)];
        }
        /* -- Tendencia positiva últimos 30 días */
        public static List<vw_ProductStarMetrics> getTendenciaPositiva(EcommersContext context)
        {
            return [.. context.vw_ProductStarMetrics
                .Where(p => p.TrendPercentage > 0).OrderByDescending(p => p.TrendPercentage)];
        }

        /*-- Mejores vendedores por categoría*/
        public static List<vw_ProductStarMetrics> getMejoresVendidos(EcommersContext context)
        {
            return [.. context.vw_ProductStarMetrics
                .Where(p => p.Sales_90_Days > 0).OrderByDescending(p => p.CategoryName).ThenByDescending(x=> x.Sales_90_Days)];
        }

        /*-- Productos con ventas decrecientes*/
        public static List<vw_ProductStarMetrics> getProductosVentasDecreciente(EcommersContext context)
        {
            return [.. context.vw_ProductStarMetrics
                .Where(p => p.TrendPercentage  < -10).OrderByDescending(p => p.TrendPercentage)];
        }

        /*-- Variantes destacadas por ventas*/
        public static List<vw_ProductStarMetrics> getVarianteDestacadaPorVentas(EcommersContext context)
        {
            return [.. context.vw_ProductStarMetrics
                .Where(p => p.StarLevel == "Gold Star" || p.StarLevel == "Silver Star" && p.StockQuantity > 0).OrderByDescending(p => p.StarScore)];
        }

        /*-- O variantes destacadas activas*/
        public static List<vw_ProductStarMetrics> getVarianteDestacadaActivas(EcommersContext context)
        {
            return [.. context.vw_ProductStarMetrics
                .Where(p => p.Sales_90_Days >= 10 && p.StockStatus == "in_stock" && p.IsActive == true )
                .OrderByDescending(p => p.StarScore)];
        }
    }
}
