using System.Security.Claims;
using AutoMapper;
using Ecommers.Application.Common.Query;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.Categorias;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Infrastructure.Queries;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommers.Application.Services
{
    public class CategoriasService(IUnitOfWork unitOfWork, IMapper mapper)
            : ICategoriasService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        // -------------------------------------------------------------------
        // GET CATEGORIAS POR DATATABLES
        // -------------------------------------------------------------------
        public async Task<DataTableResponse<CategoriesD>> GetCategoriesDataTable(
            ClaimsPrincipal user,
            DataTableRequest<CategoriesD> request)
        {
            ArgumentNullException.ThrowIfNull(user);

            var repo = _unitOfWork.Repository<CategoriesD, long>();
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
                var cantidadProductosPorCategoria = ProductsQueries.GetCountByCategories(item.Id);
                item.CantidadProductos = cantidadProductosPorCategoria;
                item.CanDelete = cantidadProductosPorCategoria == 0;
            }

            return new DataTableResponse<CategoriesD>
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
        public async Task<Result> CreateAsync(CategoriaCreateRequest request)
        {
            try
            {
                var repo = _unitOfWork.Repository<CategoriesD, long>();

                // Reordenar si es necesario
                if (request.SortOrder != null)
                {
                    var findCategorias = CategoriesQueries.GetByOrden(request.SortOrder.Value);

                    if (findCategorias != null)
                    {
                        var ordenDisponible = CategoriesQueries.FindLastOrden();
                        findCategorias.SortOrder = ordenDisponible;

                        var update = new CategoriaUpdateRequest
                        {
                            Id = findCategorias.Id,
                            Name = findCategorias.Name,
                            Slug = findCategorias.Slug,
                            Description = findCategorias.Description,
                            ShortDescription = findCategorias.ShortDescription,
                            BgClass = findCategorias.BgClass,
                            Image = findCategorias.Image,
                            SortOrder = findCategorias.SortOrder,
                            ParentId = findCategorias.ParentId
                        };

                        await UpdateInternalAsync(update);
                    }
                }

                var categories = _mapper.Map<CategoriesD>(request);
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
        public async Task<Result<CategoriesD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<CategoriesD, long>();
                var categorias = await repo.GetByIdAsync(getByIdRequest.Id);

                if (categorias == null)
                {
                    return Result<CategoriesD>.Fail("Categoría no encontrada");
                }

                categorias.CantidadProductos = ProductsQueries.GetCountByCategories(getByIdRequest.Id);

                categorias.EsNuevo = categorias.CreatedAt >= DateTime.Now.AddDays(-30);

                return Result<CategoriesD>.Ok(categorias);
            }
            catch (Exception ex)
            {
                return Result<CategoriesD>.Fail($"Error al obtener la categoría: {ex.Message}");
            }
        }

        // -------------------------------------------------------------------
        // UPDATE
        // -------------------------------------------------------------------
        public async Task<Result> UpdateAsync(CategoriaUpdateRequest request)
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
        private async Task UpdateInternalAsync(CategoriaUpdateRequest request)
        {
            var repo = _unitOfWork.Repository<CategoriesD, long>();

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
            var repo = _unitOfWork.Repository<CategoriesD, long>();

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
                var repo = _unitOfWork.Repository<CategoriesD, long>();

                var categories = await repo.GetByIdAsync(deleteRequest.Id);
                if (categories == null)
                {
                    return Result.Fail("Categoría no encontrada");
                }

                // Validar si no contiene productos asociados
                var cantidad = ProductsQueries.GetCountByCategories(deleteRequest.Id);

                if (cantidad > 0)
                {
                    return Result.Fail("No es posible eliminar esta categoría debido a que contiene productos relacionados");
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
        public async Task<IEnumerable<CategoriesD>> GetAllActiveAsync()
        {
            var repo = _unitOfWork.Repository<CategoriesD, long>();

            var categorias = await repo.GetQuery()
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();

            foreach (var item in categorias)
            {
                var cantidadProductosPorCategoria = ProductsQueries.GetCountByCategories(item.Id);
                item.CantidadProductos = cantidadProductosPorCategoria;
                item.EsNuevo = item.CreatedAt >= DateTime.Now.AddDays(-30);
            }

            return categorias;
        }

        // -------------------------------------------------------------------
        // GET BY ID - Ahora retorna Result<T>
        // -------------------------------------------------------------------
        public async Task<CategoriesD?> GetByNameAsync(long id, string name)
        {
            var repo = _unitOfWork.Repository<CategoriesD, long>();

            var categorias = await repo.GetQuery()
            .AsNoTracking()
            .Where(x => x.Name == name && x.Id != id)
            .OrderBy(x => x.SortOrder)
            .FirstOrDefaultAsync();

            if (categorias != null)
            {
                categorias.CantidadProductos = ProductsQueries.GetCountByCategories(categorias.Id);

            }

            return categorias;
        }

        public async Task<Result> ToggleEstadoAsync(long id)
        {
            var repo = _unitOfWork.Repository<CategoriesD, long>();

            // ⚠️ NO AsNoTracking
            var categorias = await repo.GetQuery()
                .FirstOrDefaultAsync(x => x.Id == id);

            if (categorias == null)
                return Result.Fail("Categoría no encontrado");

            categorias.IsActive = !categorias.IsActive;
            categorias.UpdatedAt = DateTime.UtcNow;
            if (categorias.IsActive == false)
            {
                categorias.DeletedAt = DateTime.UtcNow;
            }
            else
            {
                categorias.DeletedAt = null;
            }

            repo.Update(categorias);

            await _unitOfWork.CompleteAsync();

            return Result.Ok("Categoría editada exitosamente");
        }

    }
}