using System.Security.Claims;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.MasterAttributes;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;

namespace Ecommers.Application.Interfaces
{
    public interface IMasterAttributes
    {
        Task<DataTableResponse<MasterAttributesD>> GetMasterAttributesDataTable(
         ClaimsPrincipal user,
         DataTableRequest<MasterAttributesD> request);

        Task<Result> CreateAsync(MasterAttributesCreateRequest request);

        Task<Result<MasterAttributesD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest);

        Task<Result> UpdateAsync(MasterAttributesUpdateRequest request);

        Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest);

        Task<IEnumerable<MasterAttributesD>> GetAllActiveAsync();

        Task<MasterAttributesD?> GetByNameAsync(long id, string name);
    }
}
