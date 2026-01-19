using System.ComponentModel.DataAnnotations;
namespace Ecommers.Application.DTOs.Requests.Products
{
    public class ProductsEditRequest : ProductsBaseRequest
    {
        [Required(ErrorMessage = "El identificador es obligatorio")]
        public long Id { get; set; }
    }
}
