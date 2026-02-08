using System.ComponentModel.DataAnnotations;
using Ecommers.Application.DTOs.Requests.Products;

namespace Ecommers.Application.DTOs.Requests.ProductImages
{

    public class ProductImagesDeleteRequest
    {
        [Required]
        public long ImagenId { get; set; }
    }

}