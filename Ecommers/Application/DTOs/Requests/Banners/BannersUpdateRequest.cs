using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Ecommers.Application.DTOs.Requests.Banners
{
    public class BannersUpdateRequest : BannersBaseRequest
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        public long Id { get; set; }

    }
}


