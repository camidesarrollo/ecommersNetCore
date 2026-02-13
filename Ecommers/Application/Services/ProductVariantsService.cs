using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Ecommers.Application.Common.Mappings;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.ProductPriceHistory;
using Ecommers.Application.DTOs.Requests.ProductVariantImages;
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

            public async Task<Result<long>> EditAsync(ProductVariantsUpdateRequest request)
            {
                try
                {
                    var repo = _unitOfWork.Repository<ProductVariantsD, long>();
                    var producto = await repo.GetByIdAsync(request.Id);
    
                    if (producto == null)
                    {
                        return Result<long>.Fail("La variante del producto no fue encontrada");
                    }
    
                    _mapper.Map(request, producto);
                    producto.UpdatedAt = DateTime.UtcNow;
    
                    var (stockQuantity, stockStatus, manageStock) = ProductVariantsExtensions.DeterminarStock(producto);
                    producto.ManageStock = manageStock;
                    producto.StockQuantity = stockQuantity;
                    producto.StockStatus = stockStatus;
    
                    repo.Update(producto);
                    await _unitOfWork.CompleteAsync();
    
                    return Result<long>.Ok(producto.Id, "La variante del producto fue editada exitosamente");
                }
                catch (Exception ex)
                {
                    return Result<long>.Fail(ex.Message);
                }
        }



        public async Task<Result> ProcesarCrearVariantesProducto(List<ProductVariantsCreateRequest> productVariantsCreateRequest,string slug, long id)
        {
            var carpeta = $"Productos/{slug}";

            foreach (var variante in productVariantsCreateRequest)
            {
                variante.ProductId = id;
                var resultado = await CreateAsync(variante);
                var variantId = resultado.Data;

                if (variantId > 0)
                {
                     var historial = await _ProductPriceHistoryService.CreateAsync(new ProductPriceHistoryCreateRequest
                                                                        {
                                                                            VariantId = variantId,
                                                                            Price = variante.Price,
                                                                            CompareAtPrice = variante.CompareAtPrice,
                                                                            StartDate = DateTime.Now,
                                                                            Reason = "Creación de la variante",
                                                                            IsActive = variante.IsActive
                                                                        });

                    if(historial.Success == false)
                    {
                        // Manejar el error del historial de precios, por ejemplo, registrarlo o devolver un mensaje de error
                        return Result.Fail($"Error al crear el historial de precios para la variante: {historial.Message}");
                    }

                    var varianteImagen = await _ProductVariantImagesService.ProcesarCrearImagenesVariante(variante.ProductVariantImages, variantId, carpeta);

                    if (varianteImagen.Success == false)
                    {
                        return Result.Fail($"Error al procesar las imágenes de la variante: {varianteImagen.Message}");
                    }

                    var varianteAtributo = await _VariantAttributesService.ProcesarAtributosVariante(variante.Attributes, variantId);
                }
            }

            return Result.Ok("Variante creada exitosamente");
        }

        public async Task<Result> ProcesarEditarVariantesProducto(List<ProductVariantsUpdateRequest> ProductVariantsUpdateRequest, string slug, long id)
        {
            var carpeta = $"Productos/{slug}";

            foreach (var variante in ProductVariantsUpdateRequest)
            {
                variante.ProductId = id;
                var resultado = await EditAsync(variante);
                var variantId = variante.Id;

                if (variantId > 0)
                {

                    var historial = await _ProductPriceHistoryService.CreateAsync(new ProductPriceHistoryCreateRequest
                    {
                        VariantId = variantId,
                        Price = variante.Price,
                        CompareAtPrice = variante.CompareAtPrice,
                        StartDate = DateTime.Now,
                        Reason = "Creación de la variante",
                        IsActive = variante.IsActive
                    });

                    if (historial.Success == false)
                    {
                        // Manejar el error del historial de precios, por ejemplo, registrarlo o devolver un mensaje de error
                        return Result.Fail($"Error al crear el historial de precios para la variante: {historial.Message}");
                    }

                    var variantesImagenCrear = _mapper.Map<List<ProductVariantImagesCreateRequest>>(variante.ProductVariantImages.Where(x => x.Id == 0).ToList());

                    if (variantesImagenCrear.Count > 0)
                    {
                        var varianteImagen = await _ProductVariantImagesService.ProcesarCrearImagenesVariante(variantesImagenCrear, variantId, carpeta);

                        if (varianteImagen.Success == false)
                        {
                            return Result.Fail($"Error al procesar las imágenes de la variante: {varianteImagen.Message}");
                        }
                    }

                    var variantesImagenEditar = variante.ProductVariantImages.Where(x => x.Id != 0).ToList();

                    if (variantesImagenEditar.Count > 0)
                    {
                        var varianteImagenEditar = await _ProductVariantImagesService.ProcesarImagenEditarVariante(variantesImagenEditar, variantId, variante.Name ?? "", carpeta);

                        if (varianteImagenEditar.Success == false)
                        {
                            return Result.Fail($"Error al procesar las imágenes de la variante: {varianteImagenEditar.Message}");
                        }
                    }

                    var varianteAtributo = await _VariantAttributesService.ProcesarAtributosVariante(variante.Attributes, variantId);

                    if (varianteAtributo.Success == false)
                    {
                        return Result.Fail($"Error al procesar los atributos de la variante: {varianteAtributo.Message}");
                    }
                }
            }

            return Result.Ok("Variante creada exitosamente");
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

                // ⚠️ NO AsNoTracking
                var banner = await repo.GetQuery()
                    .FirstOrDefaultAsync(x => x.Id == getByIdRequest.Id);

                if (banner == null)
                    return Result.Fail("Variante no encontrado");

                banner.IsActive = !banner.IsActive;
                banner.UpdatedAt = DateTime.UtcNow;
                if (banner.IsActive == false)
                {
                    banner.DeletedAt = DateTime.UtcNow;
                }
                else
                {
                    banner.DeletedAt = null;
                }

                repo.Update(banner);

                await _unitOfWork.CompleteAsync();

                return Result.Ok("Variante editada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

    }
}
