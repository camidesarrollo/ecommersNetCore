using System.ComponentModel.DataAnnotations;
using Ecommers.Application.DTOs.Requests.Products;

namespace Ecommers.Application.DTOs.Requests.ProductImages
{

    public class ProductImagesUploadImageRequest
    {
        public required long ProductoId { get; set; }
        public required ProductsCreateRequest Producto { get; set; }
        public required List<ProductImagesCreateRequest> Imagenes { get; set; } = new();
    }

}