using System;
using AutoMapper;
using Azure.Core;
using Ecommers.Application.Common.Mappings;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.AttributeValues;
using Ecommers.Application.Interfaces;
using Ecommers.Application.Validator;
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

                if (!AttributeValuesExtensions.TieneAlgunValor(valorAtributos))
                {
                    return Result<long>.Fail("No es posible insertarlo porque contiene todos sus valores nulos");
                }

                valorAtributos.UpdatedAt = DateTime.UtcNow;
                valorAtributos.CreatedAt = DateTime.UtcNow;

                await repo.AddAsync(valorAtributos);
                await _unitOfWork.CompleteAsync();

                // Si el ID sigue siendo 0, buscamos manualmente con precaución de NULOS
                if (valorAtributos.Id == 0)
                {
                    var query = repo.GetQuery().Where(x => x.AttributeId == request.AttributeId);

                    // Comparaciones manuales para evitar el problema de ANSI NULLS en SQL
                    var registro = await query.ToListAsync(); // Traemos pocos registros para filtrar en memoria si es necesario

                    var match = registro.FirstOrDefault(x => (request.ValueString != null && 
                        x.ValueString == request.ValueString ) || (request.ValueDecimal != null && x.ValueDecimal == request.ValueDecimal)
                        || (request.ValueInt != null && x.ValueInt == request.ValueInt) || (request.ValueBoolean != null && x.ValueBoolean == request.ValueBoolean)
                    );

                    return Result<long>.Ok(match?.Id ?? 0, "Valor recuperado por búsqueda");
                }

                return Result<long>.Ok(valorAtributos.Id, "Valor de atributo creado exitosamente");
            }
            catch (Exception ex)
            {
                return Result<long>.Fail(ex.Message);
            }
        }

        public async Task<long> ObtenerOCrearValorAtributo(dynamic atributo, string valor)
        {
            var valorExistente = await GetByValueAsync(atributo.DataType, valor);

            if (valorExistente?.Data?.Id > 0)
                return valorExistente.Data.Id;

            var nuevoValor = AttributeValuesFromMapper.CrearNuevoValorAtributo(atributo, valor);

            var validator = new AttributeValuesCreateRequestValidator();
            var result = validator.Validate(nuevoValor);


            if (!result.IsValid)
            {
                return 0;
            }

            var resultado = await CreateAsync(nuevoValor);
            return resultado?.Data ?? 0;
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
