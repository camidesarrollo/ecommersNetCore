using AutoMapper;
using Azure.Core;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.ProductAttributes;
using Ecommers.Application.DTOs.Requests.VariantAttributes;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Domain.Extensions;
using Ecommers.Infrastructure.Persistence;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Infrastructure.Queries;
using Microsoft.AspNetCore.Mvc;
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

        // -------------------------------------------------------------------
        // EDIT
        // -------------------------------------------------------------------
        public async Task<Result> EditAsync(VariantAttributesUpdateRequest request)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductAttributesD, long>();

                var entity = await repo.GetByIdAsync(request.Id);

                if (entity == null)
                {
                    return Result.Fail("El atributo del producto no fue encontrado");
                }

                // Actualizamos propiedades
                entity.ValueId = request.ValueId;
                entity.IsActive = request.IsActive;
                entity.UpdatedAt = DateTime.UtcNow;

                repo.Update(entity);
                await _unitOfWork.CompleteAsync();

                return Result.Ok("Atributo del producto actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return Result.Fail($"Error al actualizar el atributo del producto: {ex.Message}");
            }
        }

        public Result<List<VariantAttributesD>> FindData(long ProductId, long MasterAttributeId)
        {
            try
            {
                var repo = _unitOfWork.Repository<VariantAttributesD, long>();
                // Corrección CS1718: comparar el campo AttributeId con MasterAttributeId
                var productAttributes = repo.GetQuery().AsNoTracking()
                    .Where(x => x.VariantId == ProductId && x.AttributeId == MasterAttributeId)
                    .ToList(); // Corrección CS1061: ToList() es síncrono sobre IQueryable

                if (productAttributes == null || productAttributes.Count == 0)
                {
                    return Result<List<VariantAttributesD>>.Fail("ProductAttributes no encontrada");
                }

                return Result<List<VariantAttributesD>>.Ok(productAttributes);
            }
            catch (Exception ex)
            {
                return Result<List<VariantAttributesD>>.Fail($"Error al obtener la ProductAttributes: {ex.Message}");
            }
        }

        public async Task<Result> ProcesarAtributosVariante(List<ProductVariantAttributeVM> variantsAttributes, long variantId)
        {
            var maestroAtributos = await _MasterAttributeService.GetAllActiveAsync();
            var atributosVariante = maestroAtributos.Where(x => x.AppliesTo == "variant").ToList();

            foreach (var atributo in atributosVariante)
            {
                var busquedaBD = FindData(variantId, atributo.Id);
                if (busquedaBD.Data != null && busquedaBD.Data.Count > 0)
                {
                    var atributoExistente = busquedaBD.Data.First();

                    var busqueda = variantsAttributes
                        .FirstOrDefault(x => x.MasterAttributeId == atributo.Id);

                    if (busqueda != null)
                    {
                        if (busqueda?.Value == "false" || busqueda?.Value == null || busqueda?.Value == null)
                        {
                            //Se debe eliminar de la base de datos 
                            var resultadoEliminar = await DeleteAsync(new DeleteRequest<long>
                            {
                                Id = atributoExistente.Id
                            });

                            if (resultadoEliminar.Success == false)
                            {
                                return Result.Fail(resultadoEliminar.Message);
                            }

                        }
                        else
                        {
                            var valueId = await _AtrributeValueService
                                .ObtenerOCrearValorAtributo(atributo, busqueda?.Value ?? "");

                            if (valueId > 0 && atributoExistente.ValueId != valueId)
                            {
                                var resultadoEditar = await EditAsync(new VariantAttributesUpdateRequest
                                {
                                    Id = atributoExistente.Id,
                                    ValueId = valueId,
                                    IsActive = true
                                });

                                if (resultadoEditar.Success == false)
                                {
                                    return Result.Fail(resultadoEditar.Message);
                                }
                            }
                        }

                    }
                }
                else
                {
                    var busqueda = variantsAttributes.FirstOrDefault(x => x.MasterAttributeId == atributo.Id && (x.Value != null && x.Value != "" && x.Value != "false"));

                    if (busqueda != null)
                    {
                        var valueId = await _AtrributeValueService.ObtenerOCrearValorAtributo(atributo, busqueda?.Value ?? "");

                        if (valueId > 0)
                        {
                            var resultadoCrear = await CreateAsync(new VariantAttributesCreateRequest
                            {
                                VariantId = variantId,
                                AttributeId = atributo.Id,
                                ValueId = valueId,
                                IsActive = true
                            });

                            if (resultadoCrear.Success == false)
                            {
                                return Result.Fail(resultadoCrear.Message);
                            }
                        }
                    }
                }


            }


            return Result.Ok("Proceso de atributo realizado exitosamente");
        }
    }
}
