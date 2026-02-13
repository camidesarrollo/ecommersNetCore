using System;
using System.Data;
using System.Text.RegularExpressions;
using AutoMapper;
using Azure.Core;
using Ecommers.Application.Common.Mappings;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.AttributeValues;
using Ecommers.Application.DTOs.Requests.Categorias;
using Ecommers.Application.DTOs.Requests.ProductAttributes;
using Ecommers.Application.DTOs.Requests.ProductImages;
using Ecommers.Application.DTOs.Requests.ProductPriceHistory;
using Ecommers.Application.DTOs.Requests.Products;
using Ecommers.Application.DTOs.Requests.ProductVariantImages;
using Ecommers.Application.DTOs.Requests.ProductVariants;
using Ecommers.Application.DTOs.Requests.VariantAttributes;
using Ecommers.Application.Interfaces;
using Ecommers.Application.Services;
using Ecommers.Application.Validator;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Domain.Extensions;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Infrastructure.Persistence.Repositories;
using Ecommers.Infrastructure.Web.Filters;
using Ecommers.Infrastructure.Web.Models.Products;
using Ecommers.Infrastructure.Web.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommers.Infrastructure.Web.Controllers
{
    [Route("Gestion/[controller]")]
    [ServiceFilter(typeof(ValidateModelFilter))]
    public class ProductsController(
            ICompositeViewEngine viewEngine, // 👈 AQUI
            IProducts ProductsService,
            IProductImages ProductImagesService,
            IProductAttributes ProductAttributesService,
            IProductPriceHistory ProductPriceHistoryService,
            IProductVariants ProductVariantsService,
            IVariantAttributes VariantAttributesService,
            IProductVariantImages ProductVariantImagesService,
            IMasterAttributes MasterAttributesService,
            IAttributeValues AtrributeValueService,
            ICategorias CategoriasService,
            IImageStorage imageStorage,
            IMapper mapper,
            ILogger<ProductsController> logger
        ) : BaseController(viewEngine) // 👈 AQUI

    {
        private readonly IProducts _Productservice = ProductsService;
        private readonly IProductImages _ProductImagesService = ProductImagesService;
        private readonly IProductAttributes _ProductAttributesService = ProductAttributesService;
        private readonly IProductPriceHistory _ProductPriceHistoryService = ProductPriceHistoryService;
        private readonly IProductVariants _ProductVariantsService = ProductVariantsService;
        private readonly IVariantAttributes _VariantAttributesService = VariantAttributesService;
        private readonly IProductVariantImages _ProductVariantImagesService = ProductVariantImagesService;
        private readonly IAttributeValues _AtrributeValueService = AtrributeValueService;
        private readonly IMasterAttributes _MasterAttributeService = MasterAttributesService;
        private readonly ICategorias _CategoriasService = CategoriasService;
        private readonly IImageStorage _imageStorage = imageStorage;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ProductsController> _logger = logger;
        // -------------------------------------------------------------------
        // GET: Vista principal
        // -------------------------------------------------------------------
        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/Categorias/Detalle/{id}
        // -------------------------------------------------------------------
        [HttpGet("Detalle/{id}")]
        public async Task<IActionResult> Detalle(int id)
        {
            var result = _Productservice.GetById(
                new GetByIdRequest<long> { Id = id });

            var MaestroAtributes = await _MasterAttributeService.GetAllActiveAsync();

            var ProductViewModel = new ProductsDetailsViewModel
            {
                Products = result.Data ?? new Products(),
                MasterAttributes = MaestroAtributes,
            };

            var res = Result<ProductsDetailsViewModel>.Ok(ProductViewModel);

            return HandleResultView(res, "~/Infrastructure/Web/Views/Products/Details.cshtml");
        }


        // -------------------------------------------------------------------
        // GET: /Gestion/MasterAttributes/Crear
        // -------------------------------------------------------------------
        [HttpGet("Crear")]
        public async Task<IActionResult> CreateAsync()
        {
            var vm = await BuildCreateViewModelAsync();
            return View(vm);
        }


        // -------------------------------------------------------------------
        // POST: /Gestion/Categorias/Crear
        // -------------------------------------------------------------------
        [HttpPost("Crear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
            try
            {


                var validator = new ProductsCreateRequestValidator();
                var result = validator.Validate(model.Products);

                if (!result.IsValid)
                {
                    var errores = "";
                    foreach (var error in result.Errors)
                    {
                        errores += $"{error.PropertyName}: {error.ErrorMessage}\n";
                    }

                    TempData["mensaje"] = errores;
                    TempData["estado"] = "Error";

                    return HandleResult(null, nameof(Index));
                }


                var producto = model.Products;
                producto.IsActive = true;

                // 3️⃣ Guardar producto primero para obtener el ID
                var productoCreado = await _Productservice.CreateAsync(producto);

                if (productoCreado == null)
                {

                    return HandleResult(productoCreado, nameof(Index));
                }

                if (productoCreado.Data == 0)
                {
                    return HandleResult(productoCreado, nameof(Index));
                }

                // 2. Procesar imágenes del producto
                var imagen = await _ProductImagesService.ProcesarImagenesProducto(model.ProductImagesD, producto.Slug, producto.Name, productoCreado.Data);

                if(!imagen.Success)
                {
                    return HandleResult(imagen, nameof(Index));
                }

                // 3. Procesar atributos del producto
                var atributos = await _ProductAttributesService.ProcesarAtributosProducto(model.ProductsAttributes, productoCreado.Data);

                if (!atributos.Success)
                {
                    return HandleResult(atributos, nameof(Index));

                }

                // 4. Procesar variantes del producto
                var variante = await _ProductVariantsService.ProcesarCrearVariantesProducto(model.ProductVariants, producto.Slug, productoCreado.Data);

                if (!variante.Success)
                {
                    return HandleResult(variante, nameof(Index));
                }

                return HandleResult(productoCreado, nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                TempData["mensaje"] = "Ocurrió un error al crear el producto. Por favor intente nuevamente.";
                TempData["estado"] = "Error";
                return HandleResult(null, nameof(Index));
            }
        }


        // -------------------------------------------------------------------
        // GET: /Gestion/Categorias/Editar/{id}
        // -------------------------------------------------------------------
        [HttpGet("Editar/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await BuildEditViewModelAsync(id);
            return View(vm);
        }

        // -------------------------------------------------------------------
        // POST: /Gestion/Categorias/Crear
        // -------------------------------------------------------------------
        [HttpPost("Editar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAsync(long id, ProductEditVM model)
        {
            try
            {
                var validator = new ProductsEditRequestValidator();
                var result = validator.Validate(model.Products);

                if (!result.IsValid)
                {
                    var errores = "";
                    foreach (var error in result.Errors)
                    {
                       errores += $"{error.PropertyName}: {error.ErrorMessage}\n";
                    }

                    TempData["mensaje"] = errores;
                    TempData["estado"] = "Error";

                    return HandleResult(null, nameof(Index));
                }

                var productoEditado = await _Productservice.UpdateAsync(model.Products);

                if (productoEditado.Success == false)
                {
                    return HandleResult(productoEditado, nameof(Index));
                }

                var imagenCrear = _mapper.Map<List<ProductImagesCreateRequest>>(model.ProductImagesD.Where(x => x.Id == 0).ToList());

     

                if(imagenCrear.Count > 0)
                {
                    var imagen_creada = await _ProductImagesService.ProcesarImagenesProducto(imagenCrear, model.Products.Slug, model.Products.Name, model.Products.Id);

                    if(imagen_creada.Success == false)
                    {
                        return HandleResult(imagen_creada, nameof(Index));

                    }
                }

                var imagenEditar = model.ProductImagesD.Where(x => x.Id != 0).ToList();

                if(imagenEditar.Count > 0)
                {
                    var imagen_editar = await _ProductImagesService.ProcesarImagenEditarProducto(imagenEditar, model.Products.Slug, model.Products.Name, model.Products.Id);

                    if (imagen_editar.Success == false)
                    {
                        return HandleResult(imagen_editar, nameof(Index));

                    }

                }

               var productoAtributo =  await _ProductAttributesService.ProcesarAtributosProducto(model.ProductsAttributes, model.Products.Id);

                if (productoAtributo.Success == false)
                {
                    return HandleResult(productoAtributo, nameof(Index));
                }

                var variantesCrear = _mapper.Map<List<ProductVariantsCreateRequest>>(model.ProductVariants.Where(x => x.Id == 0).ToList());

                if(variantesCrear.Count > 0)
                {
                    var variantes_creada = await _ProductVariantsService.ProcesarCrearVariantesProducto(variantesCrear, model.Products.Slug, model.Products.Id);
                    if (variantes_creada.Success == false)
                    {
                        return HandleResult(variantes_creada, nameof(Index));
                    }
                }

                var variantesEditar = model.ProductVariants.Where(x => x.Id != 0).ToList();

                if(variantesEditar.Count > 0)
                {
                    var variantes_editar = await _ProductVariantsService.ProcesarEditarVariantesProducto(variantesEditar, model.Products.Slug, model.Products.Id);
                    if (variantes_editar.Success == false)
                    {
                        return HandleResult(variantes_editar, nameof(Index));
                    }
                }

                return HandleResult(null, nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar producto");
                ModelState.AddModelError("", "Ocurrió un error al editar el producto. Por favor intente nuevamente.");
                return HandleResult(null, nameof(Index));
            }
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/Categorias/Eliminar/{id}
        // -------------------------------------------------------------------
        [HttpGet("Eliminar/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = _Productservice.GetById(
                       new GetByIdRequest<long> { Id = id });

            var MaestroAtributes = await _MasterAttributeService.GetAllActiveAsync();

            var ProductViewModel = new ProductsDetailsViewModel
            {
                Products = result.Data ?? new Products(),
                MasterAttributes = MaestroAtributes,
            };

            var res = Result<ProductsDetailsViewModel>.Ok(ProductViewModel);

            return HandleResultView(res, "~/Infrastructure/Web/Views/Products/Delete.cshtml");
        }

        // -------------------------------------------------------------------
        // POST: /Gestion/Categorias/Eliminar
        // -------------------------------------------------------------------
        [HttpPost("Eliminar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(long id, DeleteRequest<long> request)
        {
            try
            {
                var resultProducto = _Productservice.GetById(
                    new GetByIdRequest<long> { Id = id }
                );

                if (resultProducto.Data == null)
                {
                    TempData["mensaje"] = "Producto no encontrado";
                    TempData["estado"] = "Error";
                    return RedirectToAction(nameof(Index));
                }

                // ✅ 1. VALIDAR SI TIENE VENTAS
                if (resultProducto.Data.ProductVariants
                    .Any(v => v.OrderItems.Count != 0))
                {
                    TempData["mensaje"] = "No se puede eliminar el producto porque tiene ventas asociadas";
                    TempData["estado"] = "Error";
                    return RedirectToAction(nameof(Index));
                }

                // ✅ 2. ELIMINAR EN ORDEN CORRECTO (HIJOS → PADRE)

                foreach (var productVariant in resultProducto.Data.ProductVariants.ToList())
                {
                    // 2.1 VariantAttributes
                    foreach (var variantAttribute in productVariant.VariantAttributes.ToList())
                    {
                        var deleteResult = await _VariantAttributesService.DeleteAsync(
                            new DeleteRequest<long> { Id = variantAttribute.Id }
                        );

                        if (!deleteResult.Success)
                        {
                            TempData["mensaje"] = deleteResult.Message;
                            TempData["estado"] = "Error";
                            return RedirectToAction(nameof(Index));
                        }
                    }

                    // 2.2 ProductPriceHistory
                    foreach (var priceHistory in productVariant.ProductPriceHistory.ToList())
                    {
                        var deleteResult = await _ProductPriceHistoryService.DeleteAsync(
                            new DeleteRequest<long> { Id = priceHistory.Id }
                        );

                        if (!deleteResult.Success)
                        {
                            TempData["mensaje"] = deleteResult.Message;
                            TempData["estado"] = "Error";
                            return RedirectToAction(nameof(Index));
                        }
                    }

                    // 2.3 ProductVariantImages
                    foreach (var variantImage in productVariant.ProductVariantImages.ToList())
                    {
                        var deleteResult = await _ProductVariantImagesService.DeleteAsync(
                            new DeleteRequest<long> { Id = variantImage.Id }
                        );

                        if (!deleteResult.Success)
                        {
                            TempData["mensaje"] = deleteResult.Message;
                            TempData["estado"] = "Error";
                            return RedirectToAction(nameof(Index));
                        }
                    }

                    // 2.4 ProductVariant
                    var deleteVariantResult = await _ProductVariantsService.DeleteAsync(
                        new DeleteRequest<long> { Id = productVariant.Id }
                    );

                    if (!deleteVariantResult.Success)
                    {
                        TempData["mensaje"] = deleteVariantResult.Message;
                        TempData["estado"] = "Error";
                        return RedirectToAction(nameof(Index));
                    }
                }

                // 2.5 ProductAttributes
                foreach (var productAttribute in resultProducto.Data.ProductAttributes.ToList())
                {
                    var deleteResult = await _ProductAttributesService.DeleteAsync(
                        new DeleteRequest<long> { Id = productAttribute.Id }
                    );

                    if (!deleteResult.Success)
                    {
                        TempData["mensaje"] = deleteResult.Message;
                        TempData["estado"] = "Error";
                        return RedirectToAction(nameof(Index));
                    }
                }

                // 2.6 ProductImages
                foreach (var productImage in resultProducto.Data.ProductImages.ToList())
                {
                    var deleteResult = await _ProductImagesService.DeleteAsync(
                        new DeleteRequest<long> { Id = productImage.Id }
                    );

                    if (!deleteResult.Success)
                    {
                        TempData["mensaje"] = deleteResult.Message;
                        TempData["estado"] = "Error";
                        return RedirectToAction(nameof(Index));
                    }
                }

                // ✅ 3. ELIMINAR PRODUCTO
                var finalResult = await _Productservice.DeleteAsync(request);

                if (finalResult.Success)
                {
                    await _AtrributeValueService.DeleteMasivoSinUso();

                    TempData["mensaje"] = "Producto eliminado exitosamente";
                    TempData["estado"] = "Exito";
                }
                else
                {
                    TempData["mensaje"] = finalResult.Message;
                    TempData["estado"] = "Error";
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                TempData["estado"] = "Error";
                TempData["mensaje"] = $"Error al eliminar el producto: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // -------------------------------------------------------------------
        // ENDPOINT PARA DATATABLES (server-side)
        // -------------------------------------------------------------------
        [HttpPost("ObtenerProductosDataTable")]
        [IgnoreAntiforgeryToken] // DataTables no envía token CSRF
        [SkipModelValidation] // Excluir de la validación automática del filtro
        public async Task<IActionResult> GetProductosDataTable([FromForm] DataTableRequest<vw_Products> request)
        {
            try
            {
                // CRÍTICO: Validar que draw venga del request
                int draw = request.Draw > 0 ? request.Draw : 1;

                // Log para debugging
                _logger.LogInformation(
                    "DataTable Request - Draw: {Draw}, Start: {Start}, Length: {Length}",
                    draw,
                    request.Start,
                    request.Length
                );

                // Obtener los datos
                var result = await _Productservice.GetProductosDataTable(User, request);

                // Validar que result no sea null
                if (result == null)
                {
                    _logger.LogWarning("El servicio retornó null");
                    return Json(new
                    {
                        draw,
                        recordsTotal = 0,
                        recordsFiltered = 0,
                        data = new List<object>()
                    });
                }

                // DataTables exige esta estructura exacta:
                var response = new
                {
                    draw, // Usar el draw del request, NO del result
                    recordsTotal = result.TotalCount,
                    recordsFiltered = result.FilteredCount,
                    data = result.Data ?? [] // Asegurar que data nunca sea null
                };

                return Json(response);
            }
            catch (Exception ex)
            {
                // Log del error
                _logger.LogError(ex, "Error al obtener los productos para DataTables");

                // Retornar estructura válida incluso en error
                return Json(new
                {
                    draw = request.Draw > 0 ? request.Draw : 1,
                    recordsTotal = 0,
                    recordsFiltered = 0,
                    data = new List<object>(),
                    error = "Error al procesar la solicitud"
                });
            }
        }

        /// <summary>
        /// Obtiene la vista parcial de una variante de producto
        /// </summary>
        [HttpPost("ObtenerProductVariantView")]
        [IgnoreAntiforgeryToken]
        [SkipModelValidation] // Excluir de la validación automática del filtro
        public async Task<IActionResult> ObtenerProductVariantViewAsync(VariantRequestModel request)
        {
            try
            {
                // Validar el índice
                if (request == null || request.Index < 0)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Índice de variante inválido"
                    });
                }


                var MaestroAtributes = await _MasterAttributeService.GetAllActiveAsync();


                // Crear el modelo para la vista parcial
                var model = new PartialProductVariantCreateViewModel
                {
                    Index = request.Index,
                    ProductVariant = new ProductVariantsCreateRequest(),
                    MasterAttributes = MaestroAtributes,
                    AtrributeValue = await _AtrributeValueService.GetAllActiveAsync(),
                    ProductVariantImages = []
                };

                // Renderizar la vista parcial a string
                var html = RenderPartialViewToString("~/Infrastructure/Web/Views/Products/_Partial/_ProductVariantsCreate.cshtml", model);

                // Verificar que el HTML no esté vacío
                if (string.IsNullOrWhiteSpace(html))
                {
                    return Json(new
                    {
                        success = false,
                        message = "Error al renderizar la vista parcial"
                    });
                }

                return Json(new
                {
                    success = true,
                    data = html,
                    index = request.Index
                });
            }
            catch (Exception ex)
            {
                // Log el error (usar tu sistema de logging)
                Console.WriteLine($"Error en ObtenerProductVariantView: {ex.Message}");

                return Json(new
                {
                    success = false,
                    message = "Error al obtener la vista de variante",
                    error = ex.Message
                });
            }
        }

        private async Task<ProductsCreateViewModel> BuildCreateViewModelAsync(ProductCreateVM? model = null)
        {
            model ??= new ProductCreateVM();

            model.Products ??= new ProductsCreateRequest
            {
                Description = "",
                ShortDescription = ""
            };

            model.ProductImagesD ??= new List<ProductImagesCreateRequest>
            {
                new ProductImagesCreateRequest
                {
                    IsPrimary = true,
                    IsActive = true,
                    SortOrder = 1
                }
            };

            if (!model.ProductVariants.Any())
            {
                model.ProductVariants.Add(new ProductVariantsCreateRequest { Id = 0 });
            }

            return new ProductsCreateViewModel
            {
                MasterAttributes = await _MasterAttributeService.GetAllActiveAsync(),
                AtrributeValue = await _AtrributeValueService.GetAllActiveAsync(),
                Categories = await _CategoriasService.GetAllActiveAsync(),
                Products = model
            };
        }

        private async Task<ProductsEditViewModel> BuildEditViewModelAsync(long id, ProductEditVM? model = null)
        {
            model ??= new ProductEditVM();

            var result = _Productservice.GetById(
               new GetByIdRequest<long> { Id = id });

            if (result.Data != null)
            {
                model.Products = _mapper.Map<ProductsUpdateRequest>(result.Data);

                if (result.Data.ProductImages.Count > 0)
                {
                    model.ProductImagesD = _mapper.Map<List<ProductImagesUpdateRequest>>(
                       result.Data.ProductImages
                   );
                }
                else
                {
                    model.ProductImagesD =
                    [
                        new() {
                            IsPrimary = true,
                            IsActive = true,
                            SortOrder = 1
                        }
                    ];
                }


                List<ProductAttributeVM> ProductAttributeVM = [];


                foreach (var item in result.Data.ProductAttributes)
                {
                    ProductAttributeVM.Add(new Application.DTOs.Requests.ProductAttributes.ProductAttributeVM
                    {
                        MasterAttributeId = int.Parse(item.AttributeId.ToString()),
                        Value = item.Value != null ? AttributeValuesExtensions.GetDisplayValue(item.Value) : ""
                    });
                }

                model.ProductVariants = _mapper.Map<List<ProductVariantsUpdateRequest>>(
                    result.Data.ProductVariants
                );

                model.ProductsAttributes = ProductAttributeVM;

                foreach (var item in result.Data.ProductVariants)
                {
                    List<ProductVariantAttributeVM> productVariantAttributeVMs = new List<ProductVariantAttributeVM>();
                    var productVariants = model.ProductVariants.FirstOrDefault(x => x.Id == item.Id);

                    if (productVariants != null)
                    {
                        productVariants.ProductVariantImages = _mapper.Map<List<ProductVariantImagesUpdateRequest>>(
                            item.ProductVariantImages
                         );

                        foreach (var item2 in item.VariantAttributes)
                        {
                            productVariantAttributeVMs.Add(new ProductVariantAttributeVM
                            {
                                MasterAttributeId = int.Parse(item2.AttributeId.ToString()),
                                Value = item2.Value != null ? AttributeValuesExtensions.GetDisplayValue(item2.Value) : ""
                            });
                        }

                        productVariants.Attributes = productVariantAttributeVMs;
                    }



                }

            }

            return new ProductsEditViewModel
            {
                MasterAttributes = await _MasterAttributeService.GetAllActiveAsync(),
                AtrributeValue = await _AtrributeValueService.GetAllActiveAsync(),
                Categories = await _CategoriasService.GetAllActiveAsync(),
                Products = model
            };
        }

        // -------------------------------------------------------------------
        // ENDPOINT PARA SUBIR IMAGEN DEL PRODUCTO INDIVIDUAL (server-side)
        // -------------------------------------------------------------------

        [HttpPost("SubirImagenProducto")]
        public async Task<IActionResult> SubirImagenProducto(
            [FromForm] ProductImagesUploadImageRequest request)
        {
            try
            {
                var productoResult = _Productservice.GetById(
                    new GetByIdRequest<long> { Id = request.ProductoId });

                if (productoResult.Data == null)
                {
                    return Json(new { success = false, message = "Producto no encontrado" });
                }

                var result = await _ProductImagesService.ProcesarImagenesProducto(
                    request.Imagenes,
                    request.Producto.Slug,
                    request.Producto.Name,
                    request.ProductoId
                );

                return Json(new
                {
                    success = result.Success,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // -------------------------------------------------------------------
        // ENDPOINT PARA SUBIR IMAGEN VARIANTE INDIVIDUAL (server-side)
        // -------------------------------------------------------------------
        [HttpPost("SubirImagenVariante")]
        public async Task<IActionResult> SubirImagenVariante(
            [FromForm] ProductVariantImagesUploadImageRequest request)
        {
            try
            {
                var carpeta = $"Productos/{request.Slug}";

                var result = await _ProductVariantImagesService.ProcesarCrearImagenesVariante(
                    request.Imagenes,
                    request.VariantId,
                    carpeta
                );

                return Json(new
                {
                    success = result.Success,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // -------------------------------------------------------------------
        // ENDPOINT PARA ELIMINAR VARIANTE INDIVIDUAL (server-side)
        // -------------------------------------------------------------------
        [HttpPost("EliminarVariante")]
        public async Task<IActionResult> EliminarVariante(
            [FromForm] ProductVariantImagesDeleteRequest request)
        {
            var resultVariant = await _ProductVariantsService.GetDataByIdAsync(
                new GetByIdRequest<long> { Id = request.VarianteId });

            if (resultVariant.Data == null)
                return Json(new { success = false, message = "Variante no encontrada" });

            foreach (var img in resultVariant.Data.ProductVariantImages.ToList())
            {
                var deleteResult =  await _ProductVariantImagesService.DeleteAsync(
                    new DeleteRequest<long> { Id = img.Id });


                if (!deleteResult.Success)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Error en eliminar la imagen de la variante"
                    });
                }
            }

            foreach (var priceHistory in resultVariant.Data.ProductPriceHistory.ToList())
            {
                var deleteResult = await _ProductPriceHistoryService.DeleteAsync(
                    new DeleteRequest<long> { Id = priceHistory.Id }
                );

                if (!deleteResult.Success)
                { 
                    return Json(new
                    {
                        success = false,
                        message = $"Error en eliminar el hisotiral de la variante"
                    });
                }
            }

            foreach (var variantAttribute in resultVariant.Data.VariantAttributes.ToList())
            {
                var deleteResult = await _VariantAttributesService.DeleteAsync(
                    new DeleteRequest<long> { Id = variantAttribute.Id }
                );

                if (!deleteResult.Success)
                {

                    return Json(new
                    {
                        success = false,
                        message = $"Error en eliminar los atributos de la variante"
                    });
                }
            }

            var deleteVariantResult = await _ProductVariantsService.DeleteAsync(
                        new DeleteRequest<long> { Id = request.VarianteId }
                    );

            if (!deleteVariantResult.Success)
            {
                return Json(new
                {
                    success = false,
                    message = $"Error al eliminar la  variante"
                });
            }

            return Json(new
            {
                success = true,
                message = "La variante eliminada exitosamente"
            });
        }

        // -------------------------------------------------------------------
        // ENDPOINT PARA ELIMINAR IMAGEN DEL PRODUCTO INDIVIDUAL (server-side)
        // -------------------------------------------------------------------
        [HttpPost("EliminarImagenProducto")]
        public async Task<IActionResult> EliminarImagenProducto(
            [FromForm] ProductImagesDeleteRequest request)
        {
            var result = await _ProductImagesService.DeleteAsync(
                new DeleteRequest<long> { Id = request.ImagenId });

            return Json(new
            {
                success = result.Success,
                message = result.Success
                    ? "Imagen del producto eliminada exitosamente"
                    : result.Message
            });
        }

        // -------------------------------------------------------------------
        // ENDPOINT PARA ELIMINAR IMAGEN LA VARIANTE INDIVIDUAL (server-side)
        // -------------------------------------------------------------------
        [HttpPost("EliminarImagenVariante")]
        public async Task<IActionResult> EliminarImagenVariante(
            [FromForm] ProductVariantsDeleteRequest request)
        {
            var resultVariant = await _ProductVariantsService.GetDataByIdAsync(
                new GetByIdRequest<long> { Id = request.VarianteId });

            if (resultVariant.Data == null)
                return Json(new { success = false, message = "Variante no encontrada" });

            foreach (var img in resultVariant.Data.ProductVariantImages.ToList())
            {
                await _ProductVariantImagesService.DeleteAsync(
                    new DeleteRequest<long> { Id = img.Id });
            }

            return Json(new
            {
                success = true,
                message = "Imagen de la variante eliminada exitosamente"
            });
        }

        //Generar metodo para subir imagen al storage al crear o editar producto o variante individualmente

        // -------------------------------------------------------------------
        // ENDPOINT PARA MANEJAR ESTADOS PRODUCTO INDIVIDUAL (server-side)
        // -------------------------------------------------------------------
        [HttpPost("EstadoProducto")]
        public async Task<IActionResult> EstadoProducto([FromForm] ProductStateRequest request)
        {
            try
            {
                var productoResult = _Productservice.GetById(
                    new GetByIdRequest<long> { Id = request.ProductoId });

                if (!productoResult.Success || productoResult.Data == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Producto no encontrado"
                    });
                }

                // Cambiar estado del producto
                var cambiarProductoResult = await _Productservice.CambiarEstadoAsync(
                    new GetByIdRequest<long> { Id = request.ProductoId });

                if (!cambiarProductoResult.Success)
                {
                    return Json(new
                    {
                        success = false,
                        message = cambiarProductoResult.Message
                    });
                }

                // Nuevo estado del producto (ya cambiado)
                bool nuevoEstadoProducto = !productoResult.Data.IsActive;

                foreach (var variant in productoResult.Data.ProductVariants)
                {
                    var variantResult = await _ProductVariantsService.CambiarEstadoAsync(
                        new GetByIdRequest<long> { Id = variant.Id }
                    );

                    if (!variantResult.Success)
                    {
                        return Json(new
                        {
                            success = false,
                            message = $"Error en variante {variant.Id}: {variantResult.Message}"
                        });
                    }
                }

                return Json(new
                {
                    success = true,
                    message = nuevoEstadoProducto
                        ? "Producto activado exitosamente"
                        : "Producto desactivado exitosamente"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado del producto {ProductoId}",   request.ProductoId);

                return Json(new
                {
                    success = false,
                    message = "Error al cambiar el estado del producto"
                });
            }
        }

        // -------------------------------------------------------------------
        // ENDPOINT PARA MANEJAR ESTADOS VARIANTE INDIVIDUAL (server-side)
        // -------------------------------------------------------------------
        [HttpPost("EstadoVariante")]
        public async Task<IActionResult> EstadoVariante([FromForm] ProductVariantsStateRequest request)
        {
            try
            {
                var result = await _ProductVariantsService.CambiarEstadoAsync(
                    new GetByIdRequest<long> { Id = request.VarianteId }
                );

                if (result.Success)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Variante desactivada exitosamente"
                    });
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = result.Message
                    });
                }
            }
            catch (Exception ex)
            {
                // Log del error
                _logger.LogError(ex, "Error al desactivar la variante");

                return Json(new
                {
                    success = false,
                    message = "Error al desactivar la variante",
                    error = ex.Message
                });
            }
        }



    }
}
