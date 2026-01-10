using System.Security.Claims;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Interfaces
{
    public interface IProducts
    {
        Task<int> GetCountByCategoriesAsync(GetByIdRequest<long> request);

        
        Task<DataTableResponse<vw_Products>> GetProductosDataTable(
    ClaimsPrincipal user,
    DataTableRequest<vw_Products> request);

        Result<Products> GetById(GetByIdRequest<long> getByIdRequest);

        Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest);
    }
}
