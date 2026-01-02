using System.Security.Claims;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.Banners;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Interfaces
{
    public interface IBanners
    {
        Task<DataTableResponse<BannersD>> GetBannersDataTable(
            ClaimsPrincipal user,
            DataTableRequest<BannersD> request);

        Task<Result> CreateAsync(BannersCreateRequest request);

        Task<Result<BannersD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest);

        Task<Result> UpdateAsync(BannersUpdateRequest request);

        Task ReordenarAsync(long categoriaId, int nuevoOrden);

        Task<Result> DeleteAsync(DeleteRequest deleteRequest);

        Task<IEnumerable<BannersD>> GetAllActiveAsync();

        Task<BannersD?> GetByTituloAsync(long id, string name);

        Task<IEnumerable<BannersD?>> GetBySeccionAsync(string seccion, bool activo);

        Task<Result> ToggleEstadoAsync(long id);

    }
}