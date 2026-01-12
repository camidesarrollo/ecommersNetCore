using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Application.Services
{
    public class ProductImagesService(IUnitOfWork unitOfWork, IMapper mapper, IImageStorage imageStorage, EcommersContext context)
            : IProductImages
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IImageStorage _imageStorage = imageStorage;
        private readonly EcommersContext _context = context;

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

    }
}
