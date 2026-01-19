using System.ComponentModel.DataAnnotations;
using Ecommers.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Application.DTOs.Requests.MasterAttributes
{
    public class MasterAttributesUpdateRequest : MasterAttributesBaseRequest
    {
        [Required(ErrorMessage = "El Id es obligatorio.")]
        public long Id { get; set; }

    }
    
}
