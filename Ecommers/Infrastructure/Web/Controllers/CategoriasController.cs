using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.Categorias;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Ecommers.Infrastructure.Web.Controllers
{
    [Route("Gestion/[controller]")]
    [ServiceFilter(typeof(ValidateModelFilter))]
    public class CategoriasController(
        ICompositeViewEngine viewEngine,
        ICategorias categoriassService,
        IImageStorage imageStorage,
        IMapper mapper,
        ILogger<CategoriasController> logger) : BaseController(viewEngine)
    {
        private readonly ICategorias _categoriasService = categoriassService;
        private readonly IImageStorage _imageStorage = imageStorage;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CategoriasController> _logger = logger;

        // -------------------------------------------------------------------
        // GET: Vista principal
        // -------------------------------------------------------------------
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Web/Views/Categorias/Index.cshtml");
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/Categorias/Detalle/{id}
        // -------------------------------------------------------------------
        [HttpGet("Detalle/{id}")]
        public async Task<IActionResult> Detalle(int id)
        {
            var result = await _categoriasService.GetByIdAsync(
                new GetByIdRequest<long> { Id = id });

            return HandleResultView(result, "~/Web/Views/Categorias/Details.cshtml");
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/Categorias/Crear
        // -------------------------------------------------------------------
        [HttpGet("Crear")]
        public IActionResult Crear()
        {
            CategoriesD categoriass = new() { Id = 0 };
            return View("~/Web/Views/Categorias/Create.cshtml", categoriass);
        }

        // -------------------------------------------------------------------
        // POST: /Gestion/Categorias/Crear
        // -------------------------------------------------------------------
        [HttpPost("Crear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(CategoriaCreateRequest request)
        {
            // Procesar imagen si existe
            request.Image = await _imageStorage.UpdateAsync(
                request.ImageFile,
                request.Image,
                "Categorias"
            );

            var result = await _categoriasService.CreateAsync(request);
            return HandleResult(result, nameof(Index));
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/Categorias/Editar/{id}
        // -------------------------------------------------------------------
        [HttpGet("Editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var result = await _categoriasService.GetByIdAsync(
                new GetByIdRequest<long> { Id = id });

            return HandleResultView(result, "~/Web/Views/Categorias/Edit.cshtml");
        }

        // -------------------------------------------------------------------
        // POST: /Gestion/Categorias/Editar
        // -------------------------------------------------------------------
        [HttpPost("Editar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(long id, CategoriaUpdateRequest request)
        {

            // Procesar imagen si existe
            request.Image = await _imageStorage.UpdateAsync(
                request.ImageFile,
                request.Image,
                "Categorias"
            );

            var result = await _categoriasService.UpdateAsync(request);
            return HandleResult(result, nameof(Index));
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/Categorias/Eliminar/{id}
        // -------------------------------------------------------------------
        [HttpGet("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result = await _categoriasService.GetByIdAsync(
                new GetByIdRequest<long> { Id = id });

            return HandleResultView(result, "~/Web/Views/Categorias/Delete.cshtml");
        }

        // -------------------------------------------------------------------
        // POST: /Gestion/Categorias/Eliminar
        // -------------------------------------------------------------------
        [HttpPost("Eliminar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(long id, DeleteRequest<long>  request)
        {
            var result = await _categoriasService.DeleteAsync(request);
            return HandleResult(result, nameof(Index));
        }

        // -------------------------------------------------------------------
        // ENDPOINT PARA DATATABLES (server-side)
        // -------------------------------------------------------------------
        [HttpPost("ObtenerCategoriasDataTable")]
        [IgnoreAntiforgeryToken] // DataTables no envía token CSRF
        [SkipModelValidation] // Excluir de la validación automática del filtro
        public async Task<IActionResult> GetCategoriasDataTable([FromForm] DataTableRequest<CategoriesD> request)
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
                var result = await _categoriasService.GetCategoriesDataTable(User, request);

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
                _logger.LogError(ex, "Error al obtener categorias para DataTables");

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

        [HttpPost("GetByNameAsync")]
        [IgnoreAntiforgeryToken]
        [SkipModelValidation]
        public async Task<IActionResult> GetByNameAsync(long id, string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    return Json(new
                    {
                        result = (object?)null,
                        message = "El nombre de el categorias es obligatorio."
                    });
                }

                var response = await _categoriasService.GetByNameAsync(id, name);

                if (response == null)
                {
                    return Json(new
                    {
                        result = (object?)null,
                        message = "No se encontró el categorias solicitada."
                    });
                }

                return Json(new
                {
                    result = response,
                    message = "Categorias obtenida correctamente."
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    result = (object?)null,
                    message = "Ocurrió un error inesperado al procesar la solicitud."
                });
            }
        }

        [HttpPost("CambiarEstado")]
        [IgnoreAntiforgeryToken]
        [SkipModelValidation]
        public async Task<IActionResult> ToggleEstado(long id)
        {
            try
            {
                var result = await _categoriasService.ToggleEstadoAsync(id);

                return Json(new
                {
                    success = result.Success,
                    message = result.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar estado del banner");

                return Json(new
                {
                    success = false,
                    message = "Ocurrió un error inesperado"
                });
            }
        }
    }
}