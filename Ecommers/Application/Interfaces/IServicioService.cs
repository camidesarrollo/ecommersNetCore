using System.Security.Claims;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.Servicios;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;

namespace Ecommers.Application.Interfaces
{
    public interface IServicioService
    {
        Task<DataTableResponse<ServiciosD>> GetServicioDataTable(
            ClaimsPrincipal user,
            DataTableRequest<ServiciosD> request);

        Task<Result> CreateAsync(ServiciosCreateRequest request);

        Task<Result<ServiciosD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest);

        Task<Result> UpdateAsync(ServiciosUpdateRequest request);

        Task ReordenarAsync(long servicioId, int nuevoOrden);

        Task<Result> DeleteAsync(DeleteRequest deleteRequest);

        Task<IEnumerable<ServiciosD>> GetAllActiveAsync();

        Task<ServiciosD?> GetByNameAsync(long id, string name);
    }
}