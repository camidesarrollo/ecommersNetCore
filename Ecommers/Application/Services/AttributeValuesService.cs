using System;
using AutoMapper;
using Azure.Core;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.AttributeValues;
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

        public async Task<Result<AttributeValuesD>> GetByIdAsync(GetByIdRequest<long> getByIdRequest)
        {
            try
            {
                var repo = _unitOfWork.Repository<AttributeValuesD, long>();
                var categorias = await repo.GetByIdAsync(getByIdRequest.Id);

                if (categorias == null)
                {
                    return Result<AttributeValuesD>.Fail("Atributo no encontrada");
                }

                return Result<AttributeValuesD>.Ok(categorias);
            }
            catch (Exception ex)
            {
                return Result<AttributeValuesD>.Fail($"Error al obtener la atributo: {ex.Message}");
            }
        }

        public async Task<Result<AttributeValuesD>> GetByValueAsync(string tipovalor, string valor)
        {
            try
            {
                var repo = _unitOfWork.Repository<AttributeValuesD, long>();

                AttributeValuesD? atributo = null;

                switch (tipovalor.ToLowerInvariant())
                {
                    case "string":
                        atributo = await repo.GetQuery()
                            .FirstOrDefaultAsync(x =>
                                x.ValueString == valor ||
                                x.ValueText == valor);
                        break;

                    case "number":
                        if (!decimal.TryParse(valor, out var decimalValue))
                        {
                            return Result<AttributeValuesD>.Fail("El valor no es un número válido.");
                        }

                        atributo = await repo.GetQuery()
                            .FirstOrDefaultAsync(x => x.ValueDecimal == decimalValue);
                        break;

                    case "int":
                        if (!int.TryParse(valor, out var intValue))
                        {
                            return Result<AttributeValuesD>.Fail("El valor no es un entero válido.");
                        }

                        atributo = await repo.GetQuery()
                            .FirstOrDefaultAsync(x => x.ValueInt == intValue);
                        break;

                    case "boolean":
                        if (!bool.TryParse(valor, out var boolValue))
                        {
                            return Result<AttributeValuesD>.Fail("El valor no es un booleano válido.");
                        }

                        atributo = await repo.GetQuery()
                            .FirstOrDefaultAsync(x => x.ValueBoolean == boolValue);
                        break;

                    default:
                        return Result<AttributeValuesD>.Fail("Tipo de valor no soportado.");
                }

                if (atributo == null)
                {
                    return Result<AttributeValuesD>.Fail("No se encontró el valor del atributo.");
                }

                return Result<AttributeValuesD>.Ok(atributo);
            }
            catch (Exception ex)
            {
                return Result<AttributeValuesD>.Fail($"Error al obtener el atributo: {ex.Message}");
            }
        }

        public async Task<Result<long>> CreateAsync(AttributeValuesCreateRequest request)
        {
            try
            {
                var repo = _unitOfWork.Repository<AttributeValuesD, long>();
                var valorAtributos = _mapper.Map<AttributeValuesD>(request);

                var validarInsert = AttributeValuesExtensions.TieneAlgunValor(valorAtributos);

                if (!validarInsert)
                {
                    return Result<long>.Fail("No es posible insertarlo porque contiene todos sus valores nulos");
                }

                valorAtributos.UpdatedAt = DateTime.UtcNow;
                valorAtributos.CreatedAt = DateTime.UtcNow;

                await repo.AddAsync(valorAtributos);
                await _unitOfWork.CompleteAsync();

                var valorAtributoCreadoActual = await repo.GetQuery()
                .FirstOrDefaultAsync(x => x.AttributeId == request.AttributeId && 
                                          x.ValueString == request.ValueString && 
                                          x.ValueText == request.ValueText &&
                                          x.ValueDecimal == request.ValueDecimal && 
                                          x.ValueInt == request.ValueInt && 
                                          x.ValueBoolean == request.ValueBoolean && 
                                          x.DisplayOrder == request.DisplayOrder && 
                                          x.IsActive == request.IsActive);

                return Result<long>.Ok(valorAtributoCreadoActual?.Id ?? 0, "Valor de atributo creado exitosamente");
            }
            catch (Exception ex)
            {
                return Result<long>.Fail(ex.Message);
            }
        }

        public async Task DeleteMasivoSinUso()
        {
            var atributosSinUso = AttributeValuesQueries.GetAttributeValuesSinRelacion(_context);

            if(atributosSinUso.Count > 0)
            {
                foreach(var item in atributosSinUso)
                {
                    var repo = _unitOfWork.Repository<AttributeValuesD, long>();
                    var entity = await repo.GetByIdAsync(item.Id);

                    if (entity == null)
                    {
                        continue;
                    }

                    repo.Remove(entity);
                    await _unitOfWork.CompleteAsync();
                }
            }
        }

    }
}
