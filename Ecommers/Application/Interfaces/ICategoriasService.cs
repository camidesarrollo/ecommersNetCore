using System.Security.Claims;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.Categorias;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Interfaces
{
    public interface ICategoriasService
    {
        Task<DataTableResponse<CategoriesD>> GetCategoriesDataTable(
            ClaimsPrincipal user,
            DataTableRequest<CategoriesD> request);

        Task<Result> CreateAsync(CategoriaCreateRequest request);

        Task<Result<CategoriesD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest);

        Task<Result> UpdateAsync(CategoriaUpdateRequest request);

        Task ReordenarAsync(long categoriaId, int nuevoOrden);

        Task<Result> DeleteAsync(DeleteRequest deleteRequest);

        Task<IEnumerable<CategoriesD>> GetAllActiveAsync();

        Task<CategoriesD?> GetByNameAsync(long id, string name);
    }
}