using Ecommers.Application.DTOs.Common;

namespace Ecommers.Application.Interfaces
{
    public interface IProductsService
    {
        Task<int> GetCountByCategoriesAsync(GetByIdRequest<long> request);
    }
}
