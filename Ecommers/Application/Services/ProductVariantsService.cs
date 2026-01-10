using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Infrastructure.Queries;

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

    }
}
