using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Infrastructure.Queries
{
    public class vw_VariantesEnOfertaQueries
    {
        /*-- Ver todas las ofertas*/
        public static List<vw_VariantesEnOferta> Get(EcommersContext context)
        {
            return [.. context.vw_VariantesEnOferta.OrderByDescending(x => x.PorcentajeDescuento)];
        }

        /*-- Ofertas de una categoría específica*/

        public static List<vw_VariantesEnOferta> GetByCategoria(EcommersContext context, long IdCategoria)
        {
            return [.. context.vw_VariantesEnOferta.Where(x => x.CategoryId == IdCategoria).OrderByDescending(x => x.PorcentajeDescuento)];

        }

        /*-- Mejores ofertas (más de 20% descuento)*/
        public static List<vw_VariantesEnOferta> GetBestOferta(EcommersContext context, long IdCategoria)
        {
            return [.. context.vw_VariantesEnOferta.Where(x => x.PorcentajeDescuento >= 20).OrderByDescending(x => x.PorcentajeDescuento)];

        }

        /*-- Ofertas con stock bajo¨*/
        public static List<vw_VariantesEnOferta> GetLowStockOferta(EcommersContext context)
        {
            return [.. context.vw_VariantesEnOferta.Where(x => x.StockStatus == "low_stock").OrderByDescending(x => x.PorcentajeDescuento)];
        }
        /*-- Ofertas por rango de descuento*/
        public static List<vw_VariantesEnOferta> GetByDescuentoRange(EcommersContext context, decimal descuentoStart, decimal descuentoEnd)
        {
            return [.. context.vw_VariantesEnOferta.Where(x => x.PorcentajeDescuento >= descuentoStart && x.PorcentajeDescuento <= descuentoEnd).OrderByDescending(x => x.PorcentajeDescuento)];
        }

    }
}
