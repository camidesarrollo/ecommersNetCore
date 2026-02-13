using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.ProductImages;
using Ecommers.Application.DTOs.Requests.Products;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Application.Services
{
    public class ProductImagesService(ILogger<ProductImagesService> logger, IUnitOfWork unitOfWork, IMapper mapper, IImageStorage imageStorage, EcommersContext context)
            : IProductImages
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IImageStorage _imageStorage = imageStorage;
        private readonly EcommersContext _context = context;
        private readonly ILogger<ProductImagesService> _logger = logger;
        public async Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductImagesD, long>();
                var entity = await repo.GetByIdAsync(deleteRequest.Id);
                
                if (entity == null)
                {
                    return Result.Fail("La imagen del producto no fue encontrada");
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
                        // Log pero no falla la operación
                        Console.WriteLine($"No se pudo eliminar el archivo físico: {ex.Message}");
                    }
                }

                // Eliminar registro de BD
                repo.Remove(entity);
                await _unitOfWork.CompleteAsync();
                
                return Result.Ok("La imagen del producto fue eliminada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error al eliminar la imagen del producto: {ex.Message}");
            }
        }

        public async Task<IEnumerable<ProductImagesD>> GetImagesByProductoAsync(GetByIdRequest<long> getByIdRequest)
        {

            var repo = _unitOfWork.Repository<ProductImagesD, long>();

            var productImages = await repo.GetQuery()
                .AsNoTracking()
                .Where(x => x.ProductId == getByIdRequest.Id)
                .ToListAsync();


            return productImages;
        }

        // -------------------------------------------------------------------
        // CREATE
        // -------------------------------------------------------------------
        
        
        public async Task<Result> CreateAsync(ProductImagesCreateRequest request)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductImagesD, long>();

                // Reordenar si es necesario
                var findProductImages = ProductImagesQueries.GetByOrden(request.SortOrder);

                if (findProductImages != null)
                {
                    var ordenDisponible = ProductImagesQueries.FindLastOrden();
                    findProductImages.SortOrder = ordenDisponible;

                    var update = new ProductImagesUpdateRequest
                    {
                        Id = findProductImages.Id,
                        ProductId = findProductImages.ProductId,
                        Url  = findProductImages.Url,
                        AltText = findProductImages.AltText,
                        IsPrimary = findProductImages.IsPrimary,
                        SortOrder = findProductImages.SortOrder,
                        IsActive = findProductImages.IsActive,
                    };

                    await UpdateInternalAsync(update);
                }

                var ProductImages = _mapper.Map<ProductImagesD>(request);
                ProductImages.UpdatedAt = DateTime.UtcNow;

                await repo.AddAsync(ProductImages);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("ProductImages creada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public async Task<Result> ProcesarImagenesProducto(List<ProductImagesCreateRequest> imagenes, string slug, string nameProducto, long IdProducto)
        {
            var carpeta = $"Productos/{slug}";
            var imagenesGuardadas = 0;

            foreach (var img in imagenes)
            {
                if (img.ImageFile == null) continue;

                try
                {
                    var imagen = img;
                    imagen.ProductId = IdProducto;
                    imagen.AltText = string.IsNullOrWhiteSpace(imagen.AltText) ? nameProducto : imagen.AltText;
                    imagen.IsActive = true;

                    var urlImagen = await _imageStorage.UpdateAsync(img.ImageFile, null, carpeta);

                    if (!string.IsNullOrEmpty(urlImagen))
                    {
                        imagen.Url = urlImagen;
                        await CreateAsync(imagen);
                        imagenesGuardadas++;
                    }

                    return Result.Ok("Imágenes procesadas exitosamente");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error al subir imagen {img.Url} del producto {IdProducto}");
                }
            }

            if (imagenesGuardadas == 0)
                _logger.LogWarning($"Producto {IdProducto} creado sin imágenes");

            return Result.Ok("Imágenes procesadas exitosamente");
        }

        public async Task<Result> ProcesarImagenEditarProducto(
    List<ProductImagesUpdateRequest> imagenes,
    string slug,
    string nombreProducto,
    long productId)
        {
            var carpeta = $"Productos/{slug}";
            var repo = _unitOfWork.Repository<ProductImagesD, long>();

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

                            var nuevaUrl = await _imageStorage.UpdateAsync(img.ImageFile, null, carpeta);
                            if (string.IsNullOrEmpty(nuevaUrl))
                            {
                                return Result.Fail("Error al procesar las imágenes del producto");
                            }
                            existing.Url = nuevaUrl;
                        }

                        existing.AltText = string.IsNullOrWhiteSpace(img.AltText)
                            ? nombreProducto
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

                        var nuevaUrl = await _imageStorage.UpdateAsync(img.ImageFile, null, carpeta);

                        var nuevaImagen = new ProductImagesD
                        { Id = 0,
                            ProductId = productId,
                            Url = nuevaUrl,
                            AltText = string.IsNullOrWhiteSpace(img.AltText)
                                ? nombreProducto
                                : img.AltText,
                            SortOrder = img.SortOrder,
                            IsPrimary = img.IsPrimary,
                            IsActive = true,
                            UpdatedAt = DateTime.UtcNow
                        };

                        await repo.AddAsync(nuevaImagen);
                    }
                }

                await _unitOfWork.CompleteAsync();

                return Result.Ok("Imágenes actualizadas correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al editar imágenes del producto {productId}");
                return Result.Fail("Error al procesar las imágenes del producto");
            }
        }

        private async Task UpdateInternalAsync(ProductImagesUpdateRequest request)
        {
            var repo = _unitOfWork.Repository<ProductImagesD, long>();

            var ProductImages = await repo.GetQuery()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.Id)
                ?? throw new Exception("Imagen del producto no encontrada");

            _mapper.Map(request, ProductImages);
            ProductImages.UpdatedAt = DateTime.UtcNow;

            repo.Update(ProductImages);
        }

        // -------------------------------------------------------------------
        // GET BY ID - Ahora retorna Result<T>
        // -------------------------------------------------------------------
        public async Task<Result<ProductImagesD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductImagesD, long>();
                var ProductImages = await repo.GetByIdAsync(getByIdRequest.Id);

                if (ProductImages == null)
                {
                    return Result<ProductImagesD>.Fail("La imagen del producto no encontrada");
                }


                return Result<ProductImagesD>.Ok(ProductImages);
            }
            catch (Exception ex)
            {
                return Result<ProductImagesD>.Fail($"Error al obtener la imagen del producto: {ex.Message}");
            }
        }

    }
}
