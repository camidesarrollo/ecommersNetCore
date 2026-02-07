using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.ProductAttributes;
using Ecommers.Domain.Common;

namespace Ecommers.Application.Interfaces
{
    public interface IProductAttributes
    {
        Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest);

        Task<Result> CreateAsync(ProductAttributesCreateRequest request);

        Task ProcesarAtributosProducto(List<ProductAttributeVM> productsAttributes, long productId);
    }
}
