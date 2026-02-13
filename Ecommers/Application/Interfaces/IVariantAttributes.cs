using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.VariantAttributes;
using Ecommers.Domain.Common;

namespace Ecommers.Application.Interfaces
{
    public interface IVariantAttributes
    {
        Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest);

        Task<Result> CreateAsync(VariantAttributesCreateRequest request);

        Task<Result> ProcesarAtributosVariante(List<ProductVariantAttributeVM> variantsAttributes, long variantId);
    }
}
