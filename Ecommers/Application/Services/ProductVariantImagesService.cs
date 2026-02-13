using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.ProductVariantImages;
using Ecommers.Application.DTOs.Requests.ProductVariantImages;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Application.Services
{
    public class ProductVariantImagesService(ILogger<ProductVariantImagesService> logger, IUnitOfWork unitOfWork, IMapper mapper, IImageStorage imageStorage, EcommersContext context)
            : IProductVariantImages
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        private readonly IImageStorage _imageStorage = imageStorage;

        private readonly EcommersContext _context = context;

        private readonly ILogger<ProductVariantImagesService> _logger = logger;

        public async Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductVariantImagesD, long>();
                var entity = await repo.GetByIdAsync(deleteRequest.Id);
                
                if (entity == null)
                {
                    return Result.Fail("La imagen de variante no fue encontrada");
                }

                // ✅ ELIMINAR ARCHIVO FÍSICO
                if (!string.IsNullOrWhiteSpace(entity.Url))
                {
                    try
                    {
                        await _imageStorage.DeleteAsync(entity.Url);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"No se pudo eliminar el archivo físico: {ex.Message}");
                    }
                }

                // Eliminar registro de BD
                repo.Remove(entity);
                await _unitOfWork.CompleteAsync();
                
                return Result.Ok("La imagen de variante fue eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error al eliminar la imagen de variante: {ex.Message}");
            }
        }


        public async Task<IEnumerable<ProductVariantImagesD>> GetImagesByProductoAsync(GetByIdRequest<long> getByIdRequest)
        {

            var repo = _unitOfWork.Repository<ProductVariantImagesD, long>();

            var ProductVariantImages = await repo.GetQuery()
                .AsNoTracking()
                .Where(x => x.VariantId == getByIdRequest.Id)
                .ToListAsync();


            return ProductVariantImages;
        }

        public async Task<Result> CreateAsync(ProductVariantImagesCreateRequest request)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductVariantImagesD, long>();

                var ProductVariantImages = _mapper.Map<ProductVariantImagesD>(request);
                ProductVariantImages.UpdatedAt = DateTime.UtcNow;

                await repo.AddAsync(ProductVariantImages);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("ProductVariantImages creada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result> ProcesarCrearImagenesVariante(List<ProductVariantImagesCreateRequest> imagenesVariante,
    long variantId,
    string carpetaBase)
        {
    
            foreach (var imagen in imagenesVariante)
            {
                var imageRequest = new ProductVariantImagesCreateRequest
                {
                    VariantId = variantId,
                    AltText = $"Variante {variantId}",
                    SortOrder = imagen.SortOrder,
                    IsActive = true,
                    IsPrimary = true,
                    Url = await _imageStorage.UpdateAsync(
                        imagen.ImageFile,
                        null,
                        $"{carpetaBase}/variante_{variantId}") ?? ""
                };

                var result = await CreateAsync(imageRequest);

                if (!result.Success)
                {
                    return Result.Fail($"Error al crear imagen de variante: {result.Message}");
                }
                

                
            }
            return Result.Ok("Imágenes de variante procesadas exitosamente");
        }

        public async Task<Result> ProcesarImagenEditarVariante( List<ProductVariantImagesUpdateRequest> imagenes, long variantId, string nombreVariante,  string carpetaBase)
        {
            var repo = _unitOfWork.Repository<ProductVariantImagesD, long>();

            try
            {
                foreach (var img in imagenes)
                {
                    // 🔹 Caso 1: Imagen existente
                    if (img.Id > 0)
                    {
                        var existing = await repo.GetQuery()
                    .FirstOrDefaultAsync(x => x.Id == img.Id);
                        if (existing == null) continue;

                        // Si viene nuevo archivo → reemplazar
                        if (img.ImageFile != null)
                        {
                            if (!string.IsNullOrWhiteSpace(existing.Url))
                                await _imageStorage.DeleteAsync(existing.Url);

                            var nuevaUrl = await _imageStorage.UpdateAsync(img.ImageFile, null, carpetaBase);
                            if (string.IsNullOrEmpty(nuevaUrl))
                            {
                                return Result.Fail("Error al procesar las imágenes del producto");
                            }
                            existing.Url = nuevaUrl;
                        }

                        existing.AltText = string.IsNullOrWhiteSpace(img.AltText)
                            ? nombreVariante
                            : img.AltText;

                        existing.SortOrder = img.SortOrder;
                        existing.IsPrimary = img.IsPrimary;
                        existing.IsActive = true;
                        existing.UpdatedAt = DateTime.UtcNow;

                        repo.Update(existing);
                    }
                    else
                    {
                        // 🔹 Caso 2: Nueva imagen
                        if (img.ImageFile == null) continue;

                        var nuevaUrl = await _imageStorage.UpdateAsync(img.ImageFile, null, carpetaBase);

                        var nuevaImagen = new ProductVariantImagesD
                        {
                            Id = 0,
                            VariantId = variantId,
                            Url = nuevaUrl,
                            AltText = $"Variante {variantId}",
                            SortOrder = img.SortOrder,
                            IsActive = true,
                            IsPrimary = true,
                        };

                        await repo.AddAsync(nuevaImagen);
                    }
                }

                await _unitOfWork.CompleteAsync();

                return Result.Ok("Imágenes actualizadas correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al editar imágenes del la variante {variantId}");
                return Result.Fail("Error al procesar las imágenes del variante");
            }
        }


        // -------------------------------------------------------------------
        // GET BY ID - Ahora retorna Result<T>
        // -------------------------------------------------------------------
        public async Task<Result<ProductVariantImagesD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductVariantImagesD, long>();
                var ProductVariantImages = await repo.GetByIdAsync(getByIdRequest.Id);

                if (ProductVariantImages == null)
                {
                    return Result<ProductVariantImagesD>.Fail("La imagen de variante no encontrada");
                }


                return Result<ProductVariantImagesD>.Ok(ProductVariantImages);
            }
            catch (Exception ex)
            {
                return Result<ProductVariantImagesD>.Fail($"Error al obtener la imagen de variante: {ex.Message}");
            }
        }
    }
}
