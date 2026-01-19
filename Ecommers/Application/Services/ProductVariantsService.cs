using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.ProductVariants;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Domain.Extensions;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Application.Services
{
    public class ProductVariantsService(IUnitOfWork unitOfWork, IMapper mapper, EcommersContext context)
            : IProductVariants
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly EcommersContext _context = context;

        public async Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductVariantsD, long>();
                var variant = await repo.GetByIdAsync(deleteRequest.Id);

                if (variant == null)
                {
                    return Result.Fail("La variante del producto no fue encontrada");
                }

                repo.Remove(variant);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("La variante del producto fue eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error al eliminar la variante del producto: {ex.Message}");
            }
        }

        public async Task<Result<long>> CreateAsync(ProductVariantsCreateRequest request)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductVariantsD, long>();
                var productos = _mapper.Map<ProductVariantsD>(request);
                productos.UpdatedAt = DateTime.UtcNow;
                productos.CreatedAt = DateTime.UtcNow;

                var result = ProductVariantsExtensions.DeterminarStock(productos);
                productos.ManageStock = result.manageStock;
                productos.StockQuantity = result.stockQuantity;
                productos.StockStatus = result.stockStatus;

                await repo.AddAsync(productos);
                await _unitOfWork.CompleteAsync();

                var productoCreadoActual = await repo.GetQuery()
                .FirstOrDefaultAsync(x => x.SKU == request.SKU);

                return Result<long>.Ok(productoCreadoActual?.Id ?? 0, "La variante del producto creado exitosamente");
            }
            catch (Exception ex)
            {
                return Result<long>.Fail(ex.Message);
            }
        }

    }
}
