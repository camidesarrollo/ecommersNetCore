using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Ecommers.Application.Services
{
    public class ConfiguracionService(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache cache)
            : IConfiguracionService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IMemoryCache _cache = cache;

        private const string CACHE_KEY = "configuracion_app";

        // -------------------------------------------------------------------
        // CREATE
        // -------------------------------------------------------------------
        public async Task<ConfiguracionesD> CreateAsync(ConfiguracionCreateRequest request)
        {
            var repo = _unitOfWork.Repository<ConfiguracionesD, int>();

            var configuracion = _mapper.Map<ConfiguracionesD>(request);
            configuracion.UpdatedAt = DateTime.UtcNow;

            await repo.AddAsync(configuracion);
            await _unitOfWork.CompleteAsync();

            _cache.Remove(CACHE_KEY); // limpia cache

            return configuracion;
        }

        // -------------------------------------------------------------------
        // UPDATE
        // -------------------------------------------------------------------
        public async Task<ConfiguracionesD> UpdateAsync(ConfiguracionUpdateRequest request)
        {
            var repo = _unitOfWork.Repository<ConfiguracionesD, int>();

            // Buscar sin tracking
            var configuracion = await repo.GetQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id)
                ?? throw new Exception("Configuración not found");

            // Mapear y marcar como modificado
            _mapper.Map(request, configuracion);
            configuracion.UpdatedAt = DateTime.UtcNow;

            repo.Update(configuracion);
            await _unitOfWork.CompleteAsync();
            _cache.Remove(CACHE_KEY);

            return configuracion;
        }


        // -------------------------------------------------------------------
        // DELETE
        // -------------------------------------------------------------------
        public async Task<bool> DeleteAsync(DeleteRequest deleteRequest)
        {
            var repo = _unitOfWork.Repository<ConfiguracionesD, int>();

            var configuracion = await repo.GetByIdAsync(deleteRequest.Id);
            if (configuracion == null)
                return false;

            repo.Remove(configuracion);
            await _unitOfWork.CompleteAsync();

            _cache.Remove(CACHE_KEY);

            return true;
        }

        // -------------------------------------------------------------------
        // GET BY ID
        // -------------------------------------------------------------------
        public async Task<ConfiguracionesD?> GetByIdAsync(GetByIdRequest<int> getByIdRequest)
        {
            var repo = _unitOfWork.Repository<ConfiguracionesD, int>();
            return await repo.GetByIdAsync(getByIdRequest.Id);
        }

        // -------------------------------------------------------------------
        // GET ALL
        // -------------------------------------------------------------------
        public async Task<IEnumerable<ConfiguracionesD>> GetAllAsync()
        {
            var repo = _unitOfWork.Repository<ConfiguracionesD, int>();
            return await repo.GetAllAsync();
        }

        // -------------------------------------------------------------------
        // GET ACTIVE
        // (pero usando CACHE)
        // -------------------------------------------------------------------
        public async Task<ConfiguracionesD?> GetCachedAsync(ConfiguracionCacheRequest configuracionCacheRequest)
        {
            // intentar leer de cache
            _cache.TryGetValue(CACHE_KEY, out ConfiguracionesD? cached);

            // si hay cached y no hay cambios → retornar cached
            if (cached != null)
            {
                if (configuracionCacheRequest.LastClientUpdate.HasValue && cached != null &&
                    cached.UpdatedAt.HasValue && // Ensure UpdatedAt is not null  
                    cached.UpdatedAt.Value <= configuracionCacheRequest.LastClientUpdate.Value)
                {
                    return cached; // Retornamos la última versión disponible
                }
            }


            // cargar desde BD  
            var repo = _unitOfWork.Repository<ConfiguracionesD, int>();
            var list = await repo.FindAsync(x => x.IsActive == true);
            var config = list.FirstOrDefault();

            if (config != null)
            {
                _cache.Set(CACHE_KEY, config, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
                });
            }

            return config;
        }
    }
}
