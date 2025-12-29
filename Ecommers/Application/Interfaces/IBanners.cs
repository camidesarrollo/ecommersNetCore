using System.Security.Claims;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.Banners;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;

namespace Ecommers.Application.Interfaces
{
    public interface IBannersService
    {
        Task<DataTableResponse<BannersD>> GetBannersDataTable(
            ClaimsPrincipal user,
            DataTableRequest<BannersD> request);

        Task<Result> CreateAsync(BannersCreateRequest request);

        Task<Result<BannersD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest);

        Task<Result> UpdateAsync(BannersUpdateRequest request);

        Task<Result> DeleteAsync(DeleteRequest deleteRequest);

        Task<IEnumerable<BannersD>> GetAllActiveAsync();

        Task<BannersD?> GetByNameAsync(long id, string name);
    }
}