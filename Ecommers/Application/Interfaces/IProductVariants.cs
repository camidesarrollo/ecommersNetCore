using System.Threading.Tasks;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.Products;
using Ecommers.Application.DTOs.Requests.ProductVariants;
using Ecommers.Domain.Common;

namespace Ecommers.Application.Interfaces
{
    public interface IProductVariants
    {
        Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest);

        Task<Result<long>> CreateAsync(ProductVariantsCreateRequest request);

        Task ProcesarVariantesProducto(List<ProductVariantsCreateRequest> productVariantsCreateRequest,string slug, long id);
    }
}
