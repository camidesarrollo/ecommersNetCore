using System.ComponentModel.DataAnnotations;

namespace Ecommers.Application.DTOs.Requests.ProductImages
{
    public class ProductImagesUpdateRequest : ProductImagesBaseRequest
    {

        [Required(ErrorMessage = "El identificador de la imagen del producto es obligatorio.")]
        public long Id { get; set; }

    }
}
