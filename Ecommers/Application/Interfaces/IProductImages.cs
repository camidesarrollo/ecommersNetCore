using Ecommers.Application.DTOs.Common;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;

namespace Ecommers.Application.Interfaces
{
    public interface IProductImages
    {
        Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest);

        Task<IEnumerable<ProductImagesD>> GetImagesByProductoAsync(GetByIdRequest<long> getByIdRequest);
    }
}
