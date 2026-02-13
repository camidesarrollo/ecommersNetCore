using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.ProductPriceHistory;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Application.Services
{
    public class ProductPriceHistoryService(IUnitOfWork unitOfWork, IMapper mapper, EcommersContext context)
            : IProductPriceHistory
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly EcommersContext _context = context;

        public async Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductPriceHistoryD, long>();
                var entity = await repo.GetByIdAsync(deleteRequest.Id);

                if (entity == null)
                {
                    return Result.Fail("El historial de precio no fue encontrado");
                }

                repo.Remove(entity);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("El historial de precio fue eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error al eliminar el historial de precio: {ex.Message}");
            }
        }

        public async Task<Result> CambiarEstadoAsync(GetByIdRequest<long> getByIdRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductPriceHistoryD, long>();
                var entity = await repo.GetByIdAsync(getByIdRequest.Id);
                if (entity == null)
                {
                    return Result.Fail("El historial de precio no fue encontrado");
                }
                entity.IsActive = !entity.IsActive;
                entity.UpdatedAt = DateTime.UtcNow;
                repo.Update(entity);
                await _unitOfWork.CompleteAsync();
                return Result.Ok("El estado del historial de precio fue cambiado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error al cambiar el estado del historial de precio: {ex.Message}");
            }
        }

        public async Task<Result> CreateAsync(ProductPriceHistoryCreateRequest request)
        {
            try
            {
                var repoPriceHistory = _unitOfWork.Repository<ProductPriceHistoryD, long>();

                /*Validar si el precio existe y no se ha editado*/

                var precioActual = repoPriceHistory.GetQuery().AsNoTracking().Where(ph => ph.VariantId == request.VariantId && ph.IsActive && (ph.Price == request.Price && ph.CompareAtPrice == request.CompareAtPrice)).ToList();

                if(precioActual.Count == 0)
                {
                    var activePriceHistories = repoPriceHistory.GetQuery().AsNoTracking().Where(ph => ph.VariantId == request.VariantId && ph.IsActive).ToList();

                    foreach (var priceHistory in activePriceHistories)
                    {
                        priceHistory.IsActive = false;
                        priceHistory.UpdatedAt = DateTime.UtcNow;
                        priceHistory.EndDate = DateTime.UtcNow;
                        repoPriceHistory.Update(priceHistory);
                    }
                    await _unitOfWork.CompleteAsync();

                }

                var repo = _unitOfWork.Repository<ProductPriceHistoryD, long>();

                var ProductPriceHistory = _mapper.Map<ProductPriceHistoryD>(request);
                ProductPriceHistory.StartDate = DateTime.UtcNow;


                await repo.AddAsync(ProductPriceHistory);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("El historial de precio del producto creada exitosamente");

            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }
    }
}
