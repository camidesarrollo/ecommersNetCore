using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.ProductImages;
using Ecommers.Application.DTOs.Requests.Products;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;

namespace Ecommers.Application.Interfaces
{
    public interface IProductImages
    {
        Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest);

        Task<IEnumerable<ProductImagesD>> GetImagesByProductoAsync(GetByIdRequest<long> getByIdRequest);

        Task<Result> CreateAsync(ProductImagesCreateRequest request);

        Task<Result> ProcesarImagenesProducto(List<ProductImagesCreateRequest> imagenes, ProductsCreateRequest producto, long IdProducto);

        Task<Result<ProductImagesD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest);

    }
}
