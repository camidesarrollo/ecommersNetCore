using AutoMapper;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Application.Services
{
    public class AttributeValuesService(IUnitOfWork unitOfWork, IMapper mapper, EcommersContext context)
            : IAttributeValues
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly EcommersContext _context = context;


        // -------------------------------------------------------------------
        // GET ALL ACTIVE
        // -------------------------------------------------------------------
        public async Task<IEnumerable<AttributeValuesD>> GetAllActiveAsync()
        {
            var repo = _unitOfWork.Repository<AttributeValuesD, long>();

            var atributos = await repo.GetQuery()
                .AsNoTracking()
                .Where(x => x.IsActive)
                .ToListAsync();

            return atributos;
        }
    }
}
