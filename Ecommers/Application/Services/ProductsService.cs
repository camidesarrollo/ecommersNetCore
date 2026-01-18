using System.Security.Claims;
using AutoMapper;
using Ecommers.Application.Common.Query;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.MasterAttributes;
using Ecommers.Application.DTOs.Requests.Products;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Infrastructure.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Application.Services
{
    public class ProductsService(IUnitOfWork unitOfWork, IMapper mapper, EcommersContext context)
            : IProducts
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly EcommersContext _context = context;

        public Task<int> GetCountByCategoriesAsync(GetByIdRequest<long> request)
        {
            return Task.FromResult(
                ProductsQueries.GetCountByCategories(_context, request.Id)
            );
        }
        public async Task<DataTableResponse<vw_Products>> GetProductosDataTable(
            ClaimsPrincipal user,
            DataTableRequest<vw_Products> request)
        {
            ArgumentNullException.ThrowIfNull(user);
            ArgumentNullException.ThrowIfNull(request);

            // ✅ Obtener query base (IQueryable)
            var query = vw_ProductsQueries.GetByVW_Products(_context);

            // ✅ Obtener total de registros ANTES de filtrar
            var totalCount = await query.CountAsync();

            // ✅ Aplicar filtro de búsqueda
            var searchValue = (request.Search?.Value ?? "")
                .Trim()
                .ToLowerInvariant();

            if (!string.IsNullOrWhiteSpace(searchValue))
            {
                query = query.ApplySearchFilter(searchValue);
            }

            // ✅ Contar registros filtrados
            var filteredCount = await query.CountAsync();

            // ✅ Aplicar ordenamiento
            if (request.Order != null && request.Order.Count > 0)
            {
                var sortColumnIndex = request.Order[0].Column;
                var sortDirection = request.Order[0].Dir;

                if (sortColumnIndex >= 0 && sortColumnIndex < request.Columns.Count)
                {
                    var sortBy = request.Columns[sortColumnIndex].Data;
                    var ascending = sortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase);

                    if (!string.IsNullOrWhiteSpace(sortBy))
                    {
                        query = query.ApplySorting(sortBy, ascending);
                    }
                }
            }
            else
            {
                // Ordenamiento por defecto
                query = query.OrderByDescending(p => p.CreatedAt);
            }

            // ✅ Aplicar paginación
            query = query
                .Skip(request.Start)
                .Take(request.Length);

            // ✅ Ejecutar query y obtener datos
            var data = await query.ToListAsync();

            return new DataTableResponse<vw_Products>
            {
                Draw = request.Draw,
                Data = data,
                FilteredCount = filteredCount,
                TotalCount = totalCount
            };
        }

        public Result<Products> GetById(GetByIdRequest<long> getByIdRequest)
        {
            try
            {
                var productos = ProductsQueries.GetProductsById(_context, getByIdRequest.Id);

                if (productos == null)
                {
                    return Result<Products>.Fail("Producto no encontrada");
                }


                return Result<Products>.Ok(productos);
            }
            catch (Exception ex)
            {
                return Result<Products>.Fail($"Error al obtener los productos: {ex.Message}");
            }
        }

        public async Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductsD, long>();

                var products = await repo.GetByIdAsync(deleteRequest.Id);
                if (products == null)
                {
                    return Result.Fail("Producto no encontrado");
                }

                // Validar si no contiene productos asociados
                var ProductVariants = ProductVariantsQueries.GetProductVariantsByProduct(_context, deleteRequest.Id);

                if (ProductVariants.Count > 0)
                {
                    return Result.Fail("No es posible eliminar este producto debido a que contiene variantes relacionados");
                }

                var ProductAttributes = ProductAttributesQueries.GetProductAttributesByProduct(_context, deleteRequest.Id);

                if (ProductAttributes.Count > 0)
                {
                    return Result.Fail("No es posible eliminar este producto debido a que contiene atributos  relacionados");
                }

                var ProductImages = ProductImagesQueries.GetProductImagesByProduct(_context, deleteRequest.Id);

                if (ProductImages.Count > 0)
                {
                    return Result.Fail("No es posible eliminar este producto debido a que contiene imagenes relacionados");
                }


                repo.Remove(products);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("Producto eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        // -------------------------------------------------------------------
        // ========================================
        // OPCIÓN 3: Devolver solo el ID (Simple)
        // ========================================
        public async Task<Result<long>> CreateAsync(ProductsCreateRequest request)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductsD, long>();
                var productos = _mapper.Map<ProductsD>(request);
                productos.UpdatedAt = DateTime.UtcNow;
                productos.CreatedAt = DateTime.UtcNow;

                await repo.AddAsync(productos);
                await _unitOfWork.CompleteAsync();

                var productoCreadoActual = await repo.GetQuery()
                .FirstOrDefaultAsync(x => x.Slug == request.Slug);

                return Result<long>.Ok(productoCreadoActual?.Id ?? 0, "Producto creado exitosamente");
            }
            catch (Exception ex)
            {
                return Result<long>.Fail(ex.Message);
            }
        }
    }
}
