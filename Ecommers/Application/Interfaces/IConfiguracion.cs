using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.Configuracion;

namespace Ecommers.Application.Interfaces
{
    public interface IConfiguracion
    {
        Task<Domain.Entities.ConfiguracionesD> CreateAsync(ConfiguracionCreateRequest request);
        Task<Domain.Entities.ConfiguracionesD> UpdateAsync(ConfiguracionUpdateRequest request);
        Task<bool> DeleteAsync(DeleteRequest<int>  request);
        Task<Domain.Entities.ConfiguracionesD?> GetByIdAsync(GetByIdRequest<int> request);
        Task<IEnumerable<Domain.Entities.ConfiguracionesD>> GetAllAsync();
        Task<Domain.Entities.ConfiguracionesD?> GetCachedAsync(ConfiguracionCacheRequest request);
    }
}
