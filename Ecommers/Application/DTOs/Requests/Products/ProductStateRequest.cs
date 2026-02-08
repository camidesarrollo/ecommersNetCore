
using System.ComponentModel.DataAnnotations;
using Ecommers.Application.DTOs.Requests.ProductAttributes;
using Ecommers.Application.DTOs.Requests.ProductImages;
using Ecommers.Application.DTOs.Requests.ProductVariants;
namespace Ecommers.Application.DTOs.Requests.Products
{
    public class ProductStateRequest
    {
        [Required(ErrorMessage = "El identificador del producto es obligatorio")]
        public long ProductoId { get; set; }
    }
}