using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.ProductImages;
using Ecommers.Application.DTOs.Requests.ProductVariantImages;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Interfaces
{
    public interface IProductVariantImages
    {
        Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest);

        Task<IEnumerable<ProductVariantImagesD>> GetImagesByProductoAsync(GetByIdRequest<long> getByIdRequest);

        Task<Result> CreateAsync(ProductVariantImagesCreateRequest request);
    }
}
