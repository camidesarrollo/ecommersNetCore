using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.Interfaces;
using Ecommers.Application.Services;
using Ecommers.Domain.Common;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Infrastructure.Persistence.Repositories;
using Ecommers.Web.Filters;
using Ecommers.Web.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Web.Controllers
{
    [Route("Gestion/[controller]")]
    [ServiceFilter(typeof(ValidateModelFilter))]
    public class ProductsController(
        IProducts ProductsService,
        IProductImages ProductImagesService,
        IProductAttributes ProductAttributesService,
        IProductPriceHistory ProductPriceHistoryService,
        IProductVariants ProductVariantsService,
        IVariantAttributes VariantAttributesService,
        IProductVariantImages ProductVariantImagesService,
        IMasterAttributes MasterAttributesService,
        ICategorias CategoriasService,
        IImageStorage imageStorage,
        IMapper mapper,
        ILogger<ProductsController> logger) : BaseController
    {
        private readonly IProducts _Productservice = ProductsService;
        private readonly IProductImages _ProductImagesService = ProductImagesService;
        private readonly IProductAttributes _ProductAttributesService = ProductAttributesService;
        private readonly IProductPriceHistory _ProductPriceHistoryService = ProductPriceHistoryService;
        private readonly IProductVariants _ProductVariantsService = ProductVariantsService;
        private readonly IVariantAttributes _VariantAttributesService = VariantAttributesService;
        private readonly IProductVariantImages _ProductVariantImagesService = ProductVariantImagesService;
        
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
            var ProductViewModel = new ProductsCreateViewModel
            {
                MasterAttributes = MaestroAtributes,
                Categories = Categorias
            };  
            return View("~/Web/Views/Products/Create.cshtml", ProductViewModel);
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
                var resultProducto = _Productservice.GetById(new GetByIdRequest<long> { Id = id });

                if (resultProducto.Data == null)
                {
                    TempData["Error"] = "Producto no encontrado";
                    return RedirectToAction(nameof(Index));
                }

                // ✅ 1. VALIDAR SI TIENE VENTAS (usando la vista vw_Products)
                
                if (resultProducto.Data.ProductVariants.Any(x => x.OrderItems.Count > 0) == true)
                {
                    TempData["Error"] = "No se puede eliminar el producto porque tiene ventas asociadas";
                    return RedirectToAction(nameof(Index));
                }

                // ✅ 2. ELIMINAR EN ORDEN CORRECTO (de hijos a padres)

                // 2.1 Eliminar VariantAttributes (nivel más bajo)
                foreach (var productVariant in resultProducto.Data.ProductVariants)
                {
                    foreach (var variantAttribute in productVariant.VariantAttributes)
                    {
                        var deleteResult = await _VariantAttributesService.DeleteAsync(
                            new DeleteRequest<long> { Id = variantAttribute.Id }
                        );

                        if (!deleteResult.Success)
                        {
                            TempData["Error"] = deleteResult.Message;
                            return RedirectToAction(nameof(Index));
                        }
                    }

                    // 2.2 Eliminar ProductPriceHistory
                    foreach (var priceHistory in productVariant.ProductPriceHistory)
                    {
                        var deleteResult = await _ProductPriceHistoryService.DeleteAsync(
                            new DeleteRequest<long> { Id = priceHistory.Id }
                        );

                        if (!deleteResult.Success)
                        {
                            TempData["Error"] = deleteResult.Message;
                            return RedirectToAction(nameof(Index));
                        }
                    }

                    // 2.3 Eliminar ProductVariantImages
                    foreach (var variantImage in productVariant.ProductVariantImages)
                    {
                        var deleteResult = await _ProductVariantImagesService.DeleteAsync(
                            new DeleteRequest<long> { Id = variantImage.Id }
                        );

                        if (!deleteResult.Success)
                        {
                            TempData["Error"] = deleteResult.Message;
                            return RedirectToAction(nameof(Index));
                        }
                    }

                    // 2.4 Eliminar ProductVariants
                    var deleteVariantResult = await _ProductVariantsService.DeleteAsync(
                        new DeleteRequest<long> { Id = productVariant.Id }
                    );

                    if (!deleteVariantResult.Success)
                    {
                        TempData["Error"] = deleteVariantResult.Message;
                        return RedirectToAction(nameof(Index));
                    }
                }

                // 2.5 Eliminar ProductAttributes (nivel producto)
                foreach (var productAttribute in resultProducto.Data.ProductAttributes)
                {
                    var deleteResult = await _ProductAttributesService.DeleteAsync(
                        new DeleteRequest<long> { Id = productAttribute.Id }
                    );

                    if (!deleteResult.Success)
                    {
                        TempData["Error"] = deleteResult.Message;
                        return RedirectToAction(nameof(Index));
                    }
                }

                // 2.6 Eliminar ProductImages
                foreach (var productImage in resultProducto.Data.ProductImages)
                {
                    var deleteResult = await _ProductImagesService.DeleteAsync(
                        new DeleteRequest<long> { Id = productImage.Id }
                    );

                    if (!deleteResult.Success)
                    {
                        TempData["Error"] = deleteResult.Message;
                        return RedirectToAction(nameof(Index));
                    }
                }

                // ✅ 3. FINALMENTE ELIMINAR EL PRODUCTO
                var finalResult = await _Productservice.DeleteAsync(request);

                if (finalResult.Success)
                {
                    TempData["Success"] = "Producto eliminado exitosamente";
                }
                else
                {
                    TempData["Error"] = finalResult.Message;
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al eliminar el producto: {ex.Message}";
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
    }
}
