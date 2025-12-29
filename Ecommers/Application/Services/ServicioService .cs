using System.Security.Claims;
using AutoMapper;
using Ecommers.Application.Common.Query;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.Servicios;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Application.Services
{
    public class ServicioService(IUnitOfWork unitOfWork, IMapper mapper)
            : IServicioService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        // -------------------------------------------------------------------
        // GET SERVICIOS POR DATATABLES
        // -------------------------------------------------------------------
        public async Task<DataTableResponse<ServiciosD>> GetServicioDataTable(
            ClaimsPrincipal user,
            DataTableRequest<ServiciosD> request)
        {
            ArgumentNullException.ThrowIfNull(user);

            var repo = _unitOfWork.Repository<ServiciosD, long>();
            var query = repo.GetQuery();

            var searchValue = (request.Search?.Value ?? "")
                .Trim()
                .ToLowerInvariant();

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                query = IQueryableSearchExtensions.ApplySearchFilter(query, searchValue);
            }

            var totalCount = await repo.GetQuery().CountAsync();
            var filteredCount = await query.CountAsync();

            if (request.Order != null && request.Order.Count > 0)
            {
                var sortColumnIndex = request.Order[0].Column;
                var sortDirection = request.Order[0].Dir;
                var sortBy = request.Columns[sortColumnIndex].Data;
                var asc = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase);

                if (!string.IsNullOrWhiteSpace(sortBy))
                {
                    query = IQueryableSortingExtensions.ApplySorting(query, sortBy, asc);
                }
            }

            query = query
                .Skip(request.Start)
                .Take(request.Length);

            var data = await query.ToListAsync();

            return new DataTableResponse<ServiciosD>
            {
                Draw = request.Draw,
                Data = data,
                FilteredCount = filteredCount,
                TotalCount = totalCount
            };
        }

        // -------------------------------------------------------------------
        // CREATE
        // -------------------------------------------------------------------
        public async Task<Result> CreateAsync(ServiciosCreateRequest request)
        {
            try
            {
                var repo = _unitOfWork.Repository<ServiciosD, long>();

                // Reordenar si es necesario
                if (request.SortOrder != null)
                {
                    var findServicio = ServicioQueries.GetByOrden(request.SortOrder.Value);

                    if (findServicio != null)
                    {
                        var ordenDisponible = ServicioQueries.FindLastOrden();
                        findServicio.SortOrder = ordenDisponible;

                        var update = new ServiciosUpdateRequest
                        {
                            Id = findServicio.Id,
                            Name = findServicio.Name,
                            Description = findServicio.Description ?? "",
                            Image = findServicio.Image,
                            SortOrder = findServicio.SortOrder,
                        };

                        await UpdateInternalAsync(update);
                    }
                }

                var categories = _mapper.Map<ServiciosD>(request);
                categories.UpdatedAt = DateTime.UtcNow;

                await repo.AddAsync(categories);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("Categoría creada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        // -------------------------------------------------------------------
        // GET BY ID - Ahora retorna Result<T>
        // -------------------------------------------------------------------
        public async Task<Result<ServiciosD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ServiciosD, long>();
                var categorias = await repo.GetByIdAsync(getByIdRequest.Id);

                if (categorias == null)
                {
                    return Result<ServiciosD>.Fail("Categoría no encontrada");
                }


                return Result<ServiciosD>.Ok(categorias);
            }
            catch (Exception ex)
            {
                return Result<ServiciosD>.Fail($"Error al obtener la categoría: {ex.Message}");
            }
        }

        // -------------------------------------------------------------------
        // UPDATE
        // -------------------------------------------------------------------
        public async Task<Result> UpdateAsync(ServiciosUpdateRequest request)
        {
            try
            {
                // Reordenar si es necesario
                if (request.SortOrder != null)
                {
                    await ReordenarAsync(request.Id, request.SortOrder.Value);
                }

                await UpdateInternalAsync(request);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("Categoría editada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        // Método interno para actualizar sin Result
        private async Task UpdateInternalAsync(ServiciosUpdateRequest request)
        {
            var repo = _unitOfWork.Repository<ServiciosD, long>();

            var categorias = await repo.GetQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id)
                ?? throw new Exception("Categoría no encontrada");

            _mapper.Map(request, categorias);
            categorias.UpdatedAt = DateTime.UtcNow;

            repo.Update(categorias);
        }

        // -------------------------------------------------------------------
        // REORDENAR
        // -------------------------------------------------------------------
        public async Task ReordenarAsync(long categoriaId, int nuevoOrden)
        {
            var repo = _unitOfWork.Repository<ServiciosD, long>();

            var categoriaActual = await repo.GetQuery()
                .FirstOrDefaultAsync(x => x.Id == categoriaId);

            if (categoriaActual != null && categoriaActual.SortOrder != null)
            {
                int ordenActual = categoriaActual.SortOrder.Value;

                if (ordenActual == nuevoOrden)
                    return;

                if (nuevoOrden < ordenActual)
                {
                    var categoriasAfectadas = await repo.GetQuery()
                        .Where(x =>
                            x.SortOrder >= nuevoOrden &&
                            x.SortOrder < ordenActual &&
                            x.Id != categoriaId)
                        .ToListAsync();

                    foreach (var cat in categoriasAfectadas)
                    {
                        cat.SortOrder += 1;
                    }
                }
                else
                {
                    var categoriasAfectadas = await repo.GetQuery()
                        .Where(x =>
                            x.SortOrder <= nuevoOrden &&
                            x.SortOrder > ordenActual &&
                            x.Id != categoriaId)
                        .ToListAsync();

                    foreach (var cat in categoriasAfectadas)
                    {
                        cat.SortOrder -= 1;
                    }
                }

                categoriaActual.SortOrder = nuevoOrden;
            }
        }

        // -------------------------------------------------------------------
        // DELETE
        // -------------------------------------------------------------------
        public async Task<Result> DeleteAsync(DeleteRequest deleteRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ServiciosD, long>();

                var categories = await repo.GetByIdAsync(deleteRequest.Id);
                if (categories == null)
                {
                    return Result.Fail("Categoría no encontrada");
                }

                repo.Remove(categories);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("Categoría eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        // -------------------------------------------------------------------
        // GET ALL ACTIVE
        // -------------------------------------------------------------------
        public async Task<IEnumerable<ServiciosD>> GetAllActiveAsync()
        {
            var repo = _unitOfWork.Repository<ServiciosD, long>();

            var categorias = await repo.GetQuery()
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            return categorias;
        }

        // -------------------------------------------------------------------
        // GET BY ID - Ahora retorna Result<T>
        // -------------------------------------------------------------------
        public async Task<ServiciosD?> GetByNameAsync(long id, string name)
        {
            var repo = _unitOfWork.Repository<ServiciosD, long>();

            var categorias = await repo.GetQuery()
            .AsNoTracking()
            .Where(x => x.Name == name && x.Id != id)
            .OrderBy(x => x.SortOrder)
            .FirstOrDefaultAsync();

            
            return categorias;
        }
    }
}