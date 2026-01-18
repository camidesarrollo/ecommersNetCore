using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.AttributeValues;
using Ecommers.Application.DTOs.Requests.ProductAttributes;
using Ecommers.Application.DTOs.Requests.Products;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Interfaces
{
    public interface IAttributeValues
    {
        Task<IEnumerable<AttributeValuesD>> GetAllActiveAsync();

        Task<Result<AttributeValuesD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest);


        Task<Result<AttributeValuesD>> GetByValueAsync(string tipovalor, string valor);
        Task<Result<long>> CreateAsync(AttributeValuesCreateRequest request);


    }
}
