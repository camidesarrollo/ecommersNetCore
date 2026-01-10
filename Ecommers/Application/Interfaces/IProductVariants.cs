using Ecommers.Application.DTOs.Common;
using Ecommers.Domain.Common;

namespace Ecommers.Application.Interfaces
{
    public interface IProductVariants
    {
        Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest);
    }
}
