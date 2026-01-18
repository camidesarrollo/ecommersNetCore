using System;
using System.Text.RegularExpressions;
using AutoMapper;
using Azure.Core;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.AttributeValues;
using Ecommers.Application.DTOs.Requests.Categorias;
using Ecommers.Application.DTOs.Requests.ProductAttributes;
using Ecommers.Application.DTOs.Requests.ProductImages;
using Ecommers.Application.DTOs.Requests.Products;
using Ecommers.Application.Interfaces;
using Ecommers.Application.Services;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Infrastructure.Persistence.Repositories;
using Ecommers.Web.Filters;
using Ecommers.Web.Models.Products;
using Ecommers.Web.Models.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Ecommers.Web.Controllers
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
            return View("~/Web/Views/Products/Index.cshtml");
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

            return HandleResultView(res, "~/Web/Views/Products/Details.cshtml");
        }


        // -------------------------------------------------------------------
        // GET: /Gestion/MasterAttributes/Crear
        // -------------------------------------------------------------------
        [HttpGet("Crear")]
        public async Task<IActionResult> CrearAsync()
        {
            var MaestroAtributes = await _MasterAttributeService.GetAllActiveAsync();
            var Categorias = await _CategoriasService.GetAllActiveAsync();
            var producto = new Products();
            producto.ProductVariants.Add(new ProductVariants());
            var ProductViewModel = new ProductsCreateViewModel
            {
                MasterAttributes = MaestroAtributes,
                Categories = Categorias,
                Products = producto

            };
            return View("~/Web/Views/Products/Create.cshtml", ProductViewModel);
        }

        // -------------------------------------------------------------------
        // POST: /Gestion/Categorias/Crear
        // -------------------------------------------------------------------
        [HttpPost("Crear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear()
        {
            try
            {
                var form = Request.Form;
                var files = Request.Form.Files;

                var productImageFiles = Request.Form.Files
                                        .Where(f => f.Name.StartsWith("ProductImages[") &&
                                                    f.Name.EndsWith(".ImageFile"))
                                        .ToList();

                var ProductsAttributeValues = form
                                        .Where(k => k.Key.StartsWith("ProductsAttributes["))
                                        .Select(k => k.Value.ToString())
                                        .Where(v => !string.IsNullOrWhiteSpace(v))
                                        .ToList();

                var productImagesWithIndex = Request.Form.Files
                                                                .Where(f => f.Name.StartsWith("ProductImages[") &&
                                                                            f.Name.EndsWith(".ImageFile"))
                                                                .Select(f =>
                                                                {
                                                                    var match = Regex.Match(f.Name, @"ProductImages\[(\d+)\]");
                                                                    int index = match.Success ? int.Parse(match.Groups[1].Value) : -1;

                                                                    return new
                                                                    {
                                                                        Index = index,
                                                                        File = f
                                                                    };
                                                                })
                                                                .Where(x => x.Index >= 0)
                                                                .OrderBy(x => x.Index)
                                                                .ToList();
               
                // 1️⃣ Crear el producto
                var producto = new ProductsCreateRequest
                {
                    Name = form["Products.Name"],
                    Description = form["Products.Description"],
                    ShortDescription = form["Products.ShortDescription"],
                    CategoryId = int.TryParse(form["Products.CategoryId"], out var catId) ? catId : 0,
                    BasePrice = decimal.TryParse(form["Products.BasePrice"], out var price) ? price : 0,
                    Slug = form["Products.Slug"],
                    IsActive =true
                };

                // 2️⃣ Validaciones
                if (string.IsNullOrWhiteSpace(producto.Name))
                {
                    ModelState.AddModelError("Products.Name", "El nombre es obligatorio");
                }

                if (string.IsNullOrWhiteSpace(producto.Description))
                {
                    ModelState.AddModelError("Products.Description", "La descripción es obligatoria");
                }

                if (producto.CategoryId == 0)
                {
                    ModelState.AddModelError("Products.CategoryId", "Debe seleccionar una categoría");
                }

                if (producto.BasePrice <= 0)
                {
                    ModelState.AddModelError("Products.BasePrice", "El precio debe ser mayor a 0");
                }

                if (!ModelState.IsValid)
                {
                    // Recargar datos necesarios para la vista (categorías, etc.)
                    ViewBag.ErrorMessage = "Por favor corrija los errores en el formulario";
                    return View("~/Web/Views/Products/Index.cshtml");
                }

                // 3️⃣ Guardar producto primero para obtener el ID
                var productoCreado = await _Productservice.CreateAsync(producto);

                if (productoCreado == null || productoCreado.Data == 0)
                {
                    ModelState.AddModelError("", "Error al crear el producto");
                    return View("~/Web/Views/Products/Index.cshtml");
                }

                // 4️⃣ Procesar imágenes
                var productImagesFiles = files
                    .Where(f => f.Name.StartsWith("ProductImages["))
                    .GroupBy(f =>
                    {
                        var match = Regex.Match(f.Name, @"ProductImages\[(\d+)\]");
                        return match.Success ? int.Parse(match.Groups[1].Value) : -1;
                    })
                    .Where(g => g.Key >= 0)
                    .OrderBy(g => g.Key)
                    .ToDictionary(g => g.Key, g => g.ToList());

                var imagenesGuardadas = new List<ProductImagesCreateRequest>();

                foreach (var img in productImagesWithIndex)
                {
                    var file = img.File;

                    if (file.Length == 0) continue;


                    var altText = form[$"ProductImages[{img.Index}].AltText"].ToString() ?? producto.Name;
                    var sortOrder = int.TryParse(form[$"ProductImages[{img.Index}].SortOrder"], out var order)
                        ? order
                        : img.Index + 1;

                    // Generar nombre único para la imagen
                    var carpeta = $"Productos/{producto.Slug}";

                    try
                    {
                        var imagen = new ProductImagesCreateRequest
                        {
                            ProductId = productoCreado.Data,
                            AltText = string.IsNullOrWhiteSpace(altText) ? producto.Name : altText,
                            SortOrder = sortOrder,
                            IsActive = true,
                            IsPrimary = form[$"ProductImages[{img.Index}].IsPrimary"] == "true" ? true :  false
                        }; 

                        foreach(var imagenes in productImagesFiles[img.Index])

                        {
                            var guardarImagen = await _imageStorage.UpdateAsync(
                                          imagenes,
                                          imagen.Url,
                                          "Productos/" + producto.Slug);

                            if (guardarImagen != null) {
                                imagen.Url = guardarImagen;
                            }
                        }

                        // Guardar imagen en BD
                        var imagenGuardada = await _ProductImagesService.CreateAsync(imagen);
                       
                    }
                    catch (Exception ex)
                    {
                        // Log del error pero continuar con otras imágenes
                        _logger.LogError(ex, $"Error al subir imagen {img.Index} del producto {productoCreado.Data}");
                    }

                }

                // 5️⃣ Verificar que al menos se guardó una imagen
                if (imagenesGuardadas.Count == 0)
                {
                    _logger.LogWarning($"Producto {productoCreado.Data} creado sin imágenes");
                }


                List<ProductAttributesCreateRequest> productAttributes = [];

                if (ProductsAttributeValues.Count > 0)
                {
                    var MaestroAtributes = await _MasterAttributeService.GetAllActiveAsync();

                    foreach (var item in MaestroAtributes)
                    {
                        var value = form["ProductsAttributes[" + item.Id + "].Value"];

                        foreach (var valor in value)
                        {
                            if (valor != null)
                            {
                                var buscarValorIngresado = await _AtrributeValueService.GetByValueAsync(item.DataType, valor);

                                var idValor = buscarValorIngresado?.Data?.Id;
                                if (idValor == null || idValor == 0)
                                {
                                    var validaDecimal = true;
                                    if (!decimal.TryParse(valor, out var decimalValue))
                                    {
                                        validaDecimal = false;
                                    }

                                    var validaInt = true;
                                    if (!int.TryParse(valor, out var intValue))
                                    {
                                        validaInt = false;
                                    }

                                    var validaBool = true;

                                    if (!bool.TryParse(valor, out var boolValue))
                                    {
                                        validaBool = true;
                                    }

                                    var nuevoAtributo = new AttributeValuesCreateRequest
                                    {
                                        AttributeId = item.Id,
                                        ValueString = item.DataType == "string" && valor.Length <= 225 ? valor : null,
                                        ValueText = item.DataType == "text" && valor.Length > 225 ? valor : null,
                                        ValueDecimal = item.DataType == "decimal" && validaDecimal == true ? decimalValue : null,
                                        ValueInt = item.DataType == "number" && validaInt == true ? intValue : null,
                                        ValueBoolean = item.DataType == "boolean" && validaBool == true ? validaBool : false,
                                        ValueDate = null,
                                        IsActive = true
                                    };
                                    var crearAtributo = await _AtrributeValueService.CreateAsync(nuevoAtributo);
                                    idValor = crearAtributo.Data;
                                }

                                productAttributes.Add(new ProductAttributesCreateRequest
                                {
                                    ProductId = productoCreado.Data,
                                    AttributeId = item.Id,
                                    ValueId = idValor,
                                    IsActive = true
                                });

                            }

                        }

                    }

                }

                //Guardar en ProductAttributes

                foreach(var item in productAttributes)
                {
                    var productAttributesGuardada = await _ProductAttributesService.CreateAsync(item);
                }

                //Guardar en ProductVariants



                //Guardar en ProductVariantImages

                //Guardar en ProductPriceHistory

                //VariantAttributes 


                TempData["SuccessMessage"] = $"Producto '{producto.Name}' creado exitosamente con {imagenesGuardadas.Count} imagen(es)";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear producto");
                ModelState.AddModelError("", "Ocurrió un error al crear el producto. Por favor intente nuevamente.");
                return View("~/Web/Views/Products/Index.cshtml");
            }
        }
        // -------------------------------------------------------------------
        // GET: /Gestion/Categorias/Editar/{id}
        // -------------------------------------------------------------------
        [HttpGet("Editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var result = _Productservice.GetById(
                new GetByIdRequest<long> { Id = id });

            var MaestroAtributes = await _MasterAttributeService.GetAllActiveAsync();
            var Categorias = await _CategoriasService.GetAllActiveAsync();
            var ValoresAtributo = await _AtrributeValueService.GetAllActiveAsync();
            var ImagenProducto = await _ProductImagesService.GetImagesByProductoAsync(
                new GetByIdRequest<long> { Id = id });

            var ImagenProductoVariante = await _ProductVariantImagesService.GetImagesByProductoAsync(
                new GetByIdRequest<long> { Id = id });

            var ProductViewModel = new ProductsEditViewModel
            {
                Products = result?.Data ?? new Products(),
                MasterAttributes = MaestroAtributes,
                Categories = Categorias,
                AtrributeValue = ValoresAtributo,
                ProductImage = ImagenProducto,
                ProductVariantImages = ImagenProductoVariante
            };
            return View("~/Web/Views/Products/Edit.cshtml", ProductViewModel);
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/Categorias/Eliminar/{id}
        // -------------------------------------------------------------------
        [HttpGet("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
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

            return HandleResultView(res, "~/Web/Views/Products/Delete.cshtml");
        }

        // -------------------------------------------------------------------
        // POST: /Gestion/Categorias/Eliminar
        // -------------------------------------------------------------------
        [HttpPost("Eliminar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(long id, DeleteRequest<long> request)
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
                var model = new PartialProductVariantViewModel
                {
                    Index = request.Index,
                    ProductVariant = new ProductVariants(),
                    MasterAttributes = MaestroAtributes,
                    ProductVariantImages =[]
                };

                // Renderizar la vista parcial a string
                var html = RenderPartialViewToString("~/Web/Views/Products/_Partial/_ProductVariants.cshtml", model);

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
    }
}
