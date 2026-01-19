using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.ProductPriceHistory;
using Ecommers.Domain.Common;

namespace Ecommers.Application.Interfaces
{
    public interface IProductPriceHistory
    {
        Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest);

        Task<Result> CreateAsync(ProductPriceHistoryCreateRequest request);
    }
}
