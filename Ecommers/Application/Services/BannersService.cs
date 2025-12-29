using System.Security.Claims;
using AutoMapper;
using Ecommers.Application.Common.Query;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.Banners;
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
    public class BannersService(IUnitOfWork unitOfWork, IMapper mapper)
            : IBannersService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        // -------------------------------------------------------------------
        // GET CATEGORIAS POR DATATABLES
        // -------------------------------------------------------------------
        public async Task<DataTableResponse<BannersD>> GetBannersDataTable(
            ClaimsPrincipal user,
            DataTableRequest<BannersD> request)
        {
            ArgumentNullException.ThrowIfNull(user);

            var repo = _unitOfWork.Repository<BannersD, long>();
            var query = repo.GetQuery();

            var searchValue = (request.Search?.Value ?? "")
                .Trim()
                .ToLowerInvariant();

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                query = query.ApplySearchFilter(searchValue);
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
                    query = query.ApplySorting(sortBy, asc);
                }
            }

            query = query
            .Skip(request.Start)
            .Take(request.Length);

            var data = await query.ToListAsync();

            foreach (var item in data)
            {

                if (data.Count == 1)
                {
                    item.CanDelete = false;
                }
                else
                {
                    item.CanDelete = true;
                }
            }

            return new DataTableResponse<BannersD>
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
        public async Task<Result> CreateAsync(BannersCreateRequest request)
        {
            try
            {
                var repo = _unitOfWork.Repository<BannersD, long>();

                // Reordenar si es necesario
                if (request.SortOrder != null)
                {
                    var findBanners = BannersQueries.GetByOrden(request.SortOrder.Value);

                    if (findBanners != null)
                    {
                        var ordenDisponible = BannersQueries.FindLastOrden();
                        findBanners.SortOrder = ordenDisponible;

                        var update = new BannersUpdateRequest
                        {
                            Id = findBanners.Id,
                            Seccion = findBanners.Seccion,
                            AltText = findBanners.AltText,
                            BotonEnlace = findBanners.BotonEnlace,
                            BotonTexto = findBanners.BotonTexto,
                            Subtitulo = findBanners.Subtitulo,
                            Titulo = findBanners.Subtitulo,
                            Image = findBanners.Titulo,
                       
                            SortOrder = findBanners.SortOrder,
                        };

                        await UpdateInternalAsync(update);
                    }
                }

                var Banners = _mapper.Map<BannersD>(request);
                Banners.UpdatedAt = DateTime.UtcNow;

                await repo.AddAsync(Banners);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("Banners creada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        // -------------------------------------------------------------------
        // GET BY ID - Ahora retorna Result<T>
        // -------------------------------------------------------------------
        public async Task<Result<BannersD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<BannersD, long>();
                var banners = await repo.GetByIdAsync(getByIdRequest.Id);

                if (banners == null)
                {
                    return Result<BannersD>.Fail("Banners no encontrada");
                }


                return Result<BannersD>.Ok(banners);
            }
            catch (Exception ex)
            {
                return Result<BannersD>.Fail($"Error al obtener los banners: {ex.Message}");
            }
        }

        // -------------------------------------------------------------------
        // UPDATE
        // -------------------------------------------------------------------
        public async Task<Result> UpdateAsync(BannersUpdateRequest request)
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

                return Result.Ok("Banners editada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }


        // Método interno para actualizar sin Result
        private async Task UpdateInternalAsync(BannersUpdateRequest request)
        {
            var repo = _unitOfWork.Repository<BannersD, long>();

            var banners = await repo.GetQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id)
                ?? throw new Exception("Banners no encontrada");

            _mapper.Map(request, banners);
            banners.UpdatedAt = DateTime.UtcNow;

            repo.Update(banners);
        }

        // -------------------------------------------------------------------
        // REORDENAR
        // -------------------------------------------------------------------
        public async Task ReordenarAsync(long bannersId, int nuevoOrden)
        {
            var repo = _unitOfWork.Repository<BannersD, long>();

            var bannersActual = await repo.GetQuery()
                .FirstOrDefaultAsync(x => x.Id == bannersId);

            if (bannersActual != null && bannersActual.SortOrder != null)
            {
                int ordenActual = bannersActual.SortOrder;

                if (ordenActual == nuevoOrden)
                    return;

                if (nuevoOrden < ordenActual)
                {
                    var bannersAfectadas = await repo.GetQuery()
                        .Where(x =>
                            x.SortOrder >= nuevoOrden &&
                            x.SortOrder < ordenActual &&
                            x.Id != bannersId)
                        .ToListAsync();

                    foreach (var cat in bannersAfectadas)
                    {
                        cat.SortOrder += 1;
                    }
                }
                else
                {
                    var bannersAfectadas = await repo.GetQuery()
                        .Where(x =>
                            x.SortOrder <= nuevoOrden &&
                            x.SortOrder > ordenActual &&
                            x.Id != bannersId)
                        .ToListAsync();

                    foreach (var cat in bannersAfectadas)
                    {
                        cat.SortOrder -= 1;
                    }
                }

                bannersActual.SortOrder = nuevoOrden;
            }
        }

        // -------------------------------------------------------------------
        // DELETE
        // -------------------------------------------------------------------
        public async Task<Result> DeleteAsync(DeleteRequest deleteRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<BannersD, long>();

                var Banners = await repo.GetByIdAsync(deleteRequest.Id);
                if (Banners == null)
                {
                    return Result.Fail("Banners no encontrada");
                }

                var query = repo.GetQuery();


                var data = await query.ToListAsync();

                if (data.Count == 1)
                {
                    return Result.Fail("No es posible eliminar ya que debe existir siempre al menos un elemento");
                }

                repo.Remove(Banners);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("Banners eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        // -------------------------------------------------------------------
        // GET ALL ACTIVE
        // -------------------------------------------------------------------
        public async Task<IEnumerable<BannersD>> GetAllActiveAsync()
        {
            var repo = _unitOfWork.Repository<BannersD, long>();

            var banners = await repo.GetQuery()
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            return banners;
        }

        // -------------------------------------------------------------------
        // GET BY ID - Ahora retorna Result<T>
        // -------------------------------------------------------------------
        public async Task<BannersD?> GetByTituloAsync(long id, string name)
        {
            var repo = _unitOfWork.Repository<BannersD, long>();

            var banners = await repo.GetQuery()
            .AsNoTracking()
            .Where(x => x.Titulo == name && x.Id != id)
            .OrderBy(x => x.SortOrder)
            .FirstOrDefaultAsync();


            return banners;
        }
    }
}