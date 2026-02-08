using AutoMapper;
using Ecommers.Application.Common.Mappings;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.ProductPriceHistory;
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
    public class ProductVariantsService(IUnitOfWork unitOfWork, IMapper mapper, EcommersContext context, IProductPriceHistory ProductPriceHistoryService, IProductVariantImages ProductVariantImagesService, IVariantAttributes VariantAttributesService)
            : IProductVariants
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly EcommersContext _context = context;

        private readonly IProductPriceHistory _ProductPriceHistoryService = ProductPriceHistoryService;

        private readonly IProductVariantImages _ProductVariantImagesService = ProductVariantImagesService;
        private readonly IVariantAttributes _VariantAttributesService = VariantAttributesService;

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

                var (stockQuantity, stockStatus, manageStock) = ProductVariantsExtensions.DeterminarStock(productos);
                productos.ManageStock = manageStock;
                productos.StockQuantity = stockQuantity;
                productos.StockStatus = stockStatus;

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

        public async Task ProcesarVariantesProducto(List<ProductVariantsCreateRequest> productVariantsCreateRequest,string slug, long id)
        {
            var carpeta = $"Productos/{slug}";

            foreach (var variante in productVariantsCreateRequest)
            {
                variante.ProductId = id;
                var resultado = await CreateAsync(variante);
                var variantId = resultado.Data;

                if (variantId > 0)
                {
                     await _ProductPriceHistoryService.CreateAsync(new ProductPriceHistoryCreateRequest
                                                                        {
                                                                            VariantId = variantId,
                                                                            Price = variante.Price,
                                                                            CompareAtPrice = variante.CompareAtPrice,
                                                                            StartDate = DateTime.Now,
                                                                            Reason = "Creación de la variante",
                                                                            IsActive = variante.IsActive
                                                                        });

                    await _ProductVariantImagesService.ProcesarImagenesVariante(variante.ProductVariantImages, variantId, carpeta);
                    await _VariantAttributesService.ProcesarAtributosVariante(variante.Attributes, variantId);
                }
            }
        }


// -------------------------------------------------------------------
        // GET BY ID - Ahora retorna Result<T>
        // -------------------------------------------------------------------
        public async Task<Result<ProductVariantsD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductVariantsD, long>();
                var categorias = await repo.GetByIdAsync(getByIdRequest.Id);

                if (categorias == null)
                {
                    return Result<ProductVariantsD>.Fail("Variante no encontrada");
                }

                return Result<ProductVariantsD>.Ok(categorias);
            }
            catch (Exception ex)
            {
                return Result<ProductVariantsD>.Fail($"Error al obtener la variante: {ex.Message}");
            }
        }

        public async Task<Result<ProductVariants>> GetDataByIdAsync(GetByIdRequest<long> getByIdRequest)
        {
            try
            {
                var variant = ProductVariantsQueries.GetProductVariantById(_context, getByIdRequest.Id);

                if (variant == null)
                {
                    return Result<ProductVariants>.Fail("Variante no encontrada");
                }

                var variantDto = _mapper.Map<ProductVariants>(variant);
                return Result<ProductVariants>.Ok(variantDto);
            }
            catch (Exception ex)
            {
                return Result<ProductVariants>.Fail($"Error al obtener la variante: {ex.Message}");
            }
        }

        public async Task<Result> CambiarEstadoAsync(GetByIdRequest<long> getByIdRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductVariantsD, long>();

                var variant = await repo.GetByIdAsync(getByIdRequest.Id);
                if (variant == null)
                {
                    return Result.Fail("Variante no encontrada");
                }

                variant.IsActive = !variant.IsActive;
                variant.UpdatedAt = DateTime.UtcNow;

                repo.Update(variant);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("Variante del producto desactivada exitosamente");
            }catch (Exception ex)
            {
                return Result.Fail($"Error al cambiar el estado de la variante del producto: {ex.Message}");
            }
        }

    }
}
