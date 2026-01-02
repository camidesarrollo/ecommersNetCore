using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.Interfaces;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Queries;

namespace Ecommers.Application.Services
{
    public class ProductsService(IUnitOfWork unitOfWork, IMapper mapper)
            : IProducts
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public Task<int> GetCountByCategoriesAsync(GetByIdRequest<long> request)
        {
            return Task.FromResult(
                ProductsQueries.GetCountByCategories(request.Id)
            );
        }

    }
}
