using System.Security.Claims;
using AutoMapper;
using Ecommers.Application.Common.Query;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.MasterAttributes;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Application.Services
{
    public class MasterAttributesService(IUnitOfWork unitOfWork, IMapper mapper)
            : IMasterAttributes
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        // -------------------------------------------------------------------
        // GET MAESTRO DE ATRIBUTOS POR DATATABLES
        // -------------------------------------------------------------------
        public async Task<DataTableResponse<MasterAttributesD>> GetMasterAttributesDataTable(
            ClaimsPrincipal user,
            DataTableRequest<MasterAttributesD> request)
        {
            ArgumentNullException.ThrowIfNull(user);

            var repo = _unitOfWork.Repository<MasterAttributesD, long>();
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
                var cantidadProductosPorMasterAttributes = ProductsQueries.GetCountByMasterAttributes(item.Id);
                item.CantidadProductos = cantidadProductosPorMasterAttributes;
                item.CanDelete = cantidadProductosPorMasterAttributes == 0;
            }

            return new DataTableResponse<MasterAttributesD>
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
        public async Task<Result> CreateAsync(MasterAttributesCreateRequest request)
        {
            try
            {
                var repo = _unitOfWork.Repository<MasterAttributesD, long>();

                var categories = _mapper.Map<MasterAttributesD>(request);
                categories.UpdatedAt = DateTime.UtcNow;

                await repo.AddAsync(categories);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("Maestro de atributos creada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        // -------------------------------------------------------------------
        // GET BY ID - Ahora retorna Result<T>
        // -------------------------------------------------------------------
        public async Task<Result<MasterAttributesD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<MasterAttributesD, long>();
                var categorias = await repo.GetByIdAsync(getByIdRequest.Id);

                if (categorias == null)
                {
                    return Result<MasterAttributesD>.Fail("Maestro de atributos no encontrada");
                }

                categorias.CantidadProductos = ProductsQueries.GetCountByMasterAttributes(getByIdRequest.Id);

                return Result<MasterAttributesD>.Ok(categorias);
            }
            catch (Exception ex)
            {
                return Result<MasterAttributesD>.Fail($"Error al obtener el Maestro de atributos: {ex.Message}");
            }
        }

        // -------------------------------------------------------------------
        // UPDATE
        // -------------------------------------------------------------------
        public async Task<Result> UpdateAsync(MasterAttributesUpdateRequest request)
        {
            try
            {
                await UpdateInternalAsync(request);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("Maestro de atributos editada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        // Método interno para actualizar sin Result
        private async Task UpdateInternalAsync(MasterAttributesUpdateRequest request)
        {
            var repo = _unitOfWork.Repository<MasterAttributesD, long>();

            var categorias = await repo.GetQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id)
                ?? throw new Exception("Maestro de atributos no encontrada");

            _mapper.Map(request, categorias);
            categorias.UpdatedAt = DateTime.UtcNow;

            repo.Update(categorias);
        }
        // -------------------------------------------------------------------
        // DELETE
        // -------------------------------------------------------------------
        public async Task<Result> DeleteAsync(DeleteRequest deleteRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<MasterAttributesD, long>();

                var categories = await repo.GetByIdAsync(deleteRequest.Id);
                if (categories == null)
                {
                    return Result.Fail("Maestro de atributos no encontrada");
                }

                // Validar si no contiene productos asociados
                var cantidad = ProductsQueries.GetCountByMasterAttributes(deleteRequest.Id);

                if (cantidad > 0)
                {
                    return Result.Fail("No es posible eliminar esta Maestro de atributos debido a que contiene productos relacionados");
                }

                repo.Remove(categories);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("Maestro de atributos eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        // -------------------------------------------------------------------
        // GET ALL ACTIVE
        // -------------------------------------------------------------------
        public async Task<IEnumerable<MasterAttributesD>> GetAllActiveAsync()
        {
            var repo = _unitOfWork.Repository<MasterAttributesD, long>();

            var atributos = await repo.GetQuery()
                .AsNoTracking()
                .Where(x => x.IsActive)
                .OrderBy(x => x.InputType)
                .ThenBy(x => x.IsRequired)
                .ToListAsync();

            return atributos;
        }

        // -------------------------------------------------------------------
        // GET BY ID - Ahora retorna Result<T>
        // -------------------------------------------------------------------
        public async Task<MasterAttributesD?> GetByNameAsync(long id, string name)
        {
            var repo = _unitOfWork.Repository<MasterAttributesD, long>();

            var categorias = await repo.GetQuery()
            .AsNoTracking()
            .Where(x => x.Name == name && x.Id != id)
            .OrderBy(x => x.Name)
            .FirstOrDefaultAsync();

            if (categorias != null)
            {
                categorias.CantidadProductos = ProductsQueries.GetCountByMasterAttributes(categorias.Id);

            }

            return categorias;
        }
    }
}
