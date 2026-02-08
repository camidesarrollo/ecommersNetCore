using System.Threading.Tasks;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.Products;
using Ecommers.Application.DTOs.Requests.ProductVariants;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Interfaces
{
    public interface IProductVariants
    {
        Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest);

        Task<Result<long>> CreateAsync(ProductVariantsCreateRequest request);

        Task ProcesarVariantesProducto(List<ProductVariantsCreateRequest> productVariantsCreateRequest,string slug, long id);

        Task<Result<ProductVariantsD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest);

        Task<Result<ProductVariants>> GetDataByIdAsync(GetByIdRequest<long> getByIdRequest);

        Task<Result> CambiarEstadoAsync(GetByIdRequest<long> getByIdRequest);
    }
}
