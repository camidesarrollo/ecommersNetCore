using Ecommers.Application.DTOs.Common;

namespace Ecommers.Application.Interfaces
{
    public interface IProducts
    {
        Task<int> GetCountByCategoriesAsync(GetByIdRequest<long> request);
    }
}
