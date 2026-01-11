using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Interfaces
{
    public interface IAttributeValues
    {
        Task<IEnumerable<AttributeValuesD>> GetAllActiveAsync();
    }
}
