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
using Microsoft.EntityFrameworkCore;

namespace Ecommers.Application.Services
{
    public class ProductAttributesService(IUnitOfWork unitOfWork, IMapper mapper, EcommersContext context, IMasterAttributes MasterAttributesService,
            IAttributeValues AtrributeValueService)
            : IProductAttributes
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

        // -------------------------------------------------------------------
        // EDIT
        // -------------------------------------------------------------------
        public async Task<Result> EditAsync(ProductAttributesUpdateRequest request)
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

        // -------------------------------------------------------------------
        // GET BY ID - Ahora retorna Result<T>
        // -------------------------------------------------------------------
        public Result<List<ProductAttributesD>> FindData(long ProductId, long MasterAttributeId)
        {
            try
            {
                var repo = _unitOfWork.Repository<ProductAttributesD, long>();
                // Corrección CS1718: comparar el campo AttributeId con MasterAttributeId
                var productAttributes = repo.GetQuery().AsNoTracking()
                    .Where(x => x.ProductId == ProductId && x.AttributeId == MasterAttributeId)
                    .ToList(); // Corrección CS1061: ToList() es síncrono sobre IQueryable

                if (productAttributes == null || productAttributes.Count == 0)
                {
                    return Result<List<ProductAttributesD>>.Fail("ProductAttributes no encontrada");
                }

                return Result<List<ProductAttributesD>>.Ok(productAttributes);
            }
            catch (Exception ex)
            {
                return Result<List<ProductAttributesD>>.Fail($"Error al obtener la ProductAttributes: {ex.Message}");
            }
        }


        public async Task<Result> ProcesarAtributosProducto(List<ProductAttributeVM> productsAttributes, long productId)
        {
            var maestroAtributos = await _MasterAttributeService.GetAllActiveAsync();
            var atributosProducto = maestroAtributos.Where(x => x.AppliesTo == "product").ToList();
            
            foreach (var atributo in atributosProducto)
            {
                var busquedaBD = FindData(productId, atributo.Id);
                if (busquedaBD.Data != null && busquedaBD.Data.Count > 0)
                {
                    var atributoExistente = busquedaBD.Data.First();

                    var busqueda = productsAttributes
                        .FirstOrDefault(x => x.MasterAttributeId == atributo.Id);

                    if (busqueda != null)
                    {
                        if(busqueda?.Value == "false" || busqueda?.Value == null || busqueda?.Value == null)
                        {
                            //Se debe eliminar de la base de datos 
                            var resultadoEliminar = await DeleteAsync(new DeleteRequest<long>
                            {
                                Id = atributoExistente.Id
                            });

                            if(resultadoEliminar.Success == false)
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
                                var resultadoEditar = await EditAsync(new ProductAttributesUpdateRequest
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
                    var busqueda = productsAttributes.FirstOrDefault(x => x.MasterAttributeId == atributo.Id && (x.Value != null && x.Value != "" && x.Value != "false"));

                    if (busqueda != null)
                    {
                        var valueId = await _AtrributeValueService.ObtenerOCrearValorAtributo(atributo, busqueda?.Value ?? "");

                        if (valueId > 0)
                        {
                            var resultadoCrear = await CreateAsync(new ProductAttributesCreateRequest
                            {
                                ProductId = productId,
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
