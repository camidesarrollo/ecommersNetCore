using AutoMapper;
using Azure.Core;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.VariantAttributes;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Domain.Extensions;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Queries;
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Application.Services
{
    public class VariantAttributesService(IUnitOfWork unitOfWork, IMapper mapper, EcommersContext context, IMasterAttributes MasterAttributesService,
            IAttributeValues AtrributeValueService)
            : IVariantAttributes
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly EcommersContext _context = context;
        private readonly IAttributeValues _AtrributeValueService = AtrributeValueService;
        private readonly IMasterAttributes _MasterAttributeService = MasterAttributesService;

        public async Task<Result> DeleteAsync(DeleteRequest<long> deleteRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<VariantAttributesD, long>();
                var entity = await repo.GetByIdAsync(deleteRequest.Id);

                if (entity == null)
                {
                    return Result.Fail("El atributo de variante no fue encontrado");
                }

                repo.Remove(entity);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("El atributo de variante fue eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error al eliminar el atributo de variante: {ex.Message}");
            }
        }
        public async Task<Result> CreateAsync(VariantAttributesCreateRequest request)
        {
            try
            {

                var repo = _unitOfWork.Repository<VariantAttributesD, long>();

                var VariantAttributes = _mapper.Map<VariantAttributesD>(request);
                VariantAttributes.UpdatedAt = DateTime.UtcNow;

                await repo.AddAsync(VariantAttributes);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("VariantAttributes creada exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail(ex.Message);
            }
        }

        public async Task ProcesarAtributosVariante(IFormCollection form, int variantIndex, long variantId)
        {
            var maestroAtributos = await _MasterAttributeService.GetAllActiveAsync();
            var atributosVariante = maestroAtributos.Where(x => x.AppliesTo == "variant").ToList();

            foreach (var atributo in atributosVariante)
            {
                var valores = form
                    .Where(k => k.Key.StartsWith($"ProductVariants[{variantIndex}].Attributes[{atributo.Id}]"))
                    .Select(k => k.Value.ToString())
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .ToList();

                foreach (var valor in valores)
                {
                    var valueId = await _AtrributeValueService.ObtenerOCrearValorAtributo(atributo, valor);

                    if (valueId > 0)
                    {
                        await CreateAsync(new VariantAttributesCreateRequest
                        {
                            VariantId = variantId,
                            AttributeId = atributo.Id,
                            ValueId = valueId,
                            IsActive = true
                        });
                    }
                }
            }
        }
    }
}
