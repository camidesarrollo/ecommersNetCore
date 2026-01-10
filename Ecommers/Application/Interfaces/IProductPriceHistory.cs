using Ecommers.Application.DTOs.Common;
using Ecommers.Domain.Common;

namespace Ecommers.Application.Interfaces
{
    public interface IProductPriceHistory
    {
        Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest);
    }
}
