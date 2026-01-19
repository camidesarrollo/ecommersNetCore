using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.AttributeValues;
using Ecommers.Application.DTOs.Requests.ProductAttributes;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Domain.Extensions;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Queries;

namespace Ecommers.Application.Services
{
    public class ProductAttributesService(IUnitOfWork unitOfWork, IMapper mapper, EcommersContext context)
            : IProductAttributes
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly EcommersContext _context = context;

        public async Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductAttributesD, long>();
                var entity = await repo.GetByIdAsync(deleteRequest.Id);

                if (entity == null)
                {
                    return Result.Fail("El atributo del producto no fue encontrado");
                }

                repo.Remove(entity);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("El atributo del producto fue eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error al eliminar el atributo del producto: {ex.Message}");
            }
        }

        // -------------------------------------------------------------------
        // CREATE
        // -------------------------------------------------------------------
        public async Task<Result> CreateAsync(ProductAttributesCreateRequest request)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductAttributesD, long>();
                var productAttributes = _mapper.Map<ProductAttributesD>(request);
                productAttributes.UpdatedAt = DateTime.UtcNow;

                await repo.AddAsync(productAttributes);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("atributo del producto creado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
