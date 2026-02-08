using System.Security.Claims;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.Categorias;
using Ecommers.Application.DTOs.Requests.Products;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Interfaces
{
    public interface IProducts
    {
        Task<int> GetCountByCategoriesAsync(GetByIdRequest<long> request);

        Task<Result<long>> CreateAsync(ProductsCreateRequest request);

        Task<DataTableResponse<vw_Products>> GetProductosDataTable(
    ClaimsPrincipal user,
    DataTableRequest<vw_Products> request);

        Result<Products> GetById(GetByIdRequest<long> getByIdRequest);

        Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest);
        Task<Result> CambiarEstadoAsync(GetByIdRequest<long> getByIdRequest);
        
    }
}
