using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.Banners;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Entities;
using Ecommers.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Web.Controllers
{
    [Route("Configuracion/[controller]")]
    [ServiceFilter(typeof(ValidateModelFilter))]
    public class BannersController(
        IBanners bannerssService,
        IImageStorage imageStorage,
        IMapper mapper,
        ILogger<BannersController> logger) : BaseController
    {
        private readonly IBanners _bannersService = bannerssService;
        private readonly IImageStorage _imageStorage = imageStorage;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<BannersController> _logger = logger;

        // -------------------------------------------------------------------
        // GET: Vista principal
        // -------------------------------------------------------------------
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Web/Views/Banners/Index.cshtml");
        }

        // -------------------------------------------------------------------
        // GET: /Configuracion/Banners/Detalle/{id}
        // -------------------------------------------------------------------
        [HttpGet("Detalle/{id}")]
        public async Task<IActionResult> Detalle(int id)
        {
            var result = await _bannersService.GetByIdAsync(
                new GetByIdRequest<long> { Id = id });

            return HandleResultView(result, "~/Web/Views/Banners/Details.cshtml");
        }

        // -------------------------------------------------------------------
        // GET: /Configuracion/Banners/Crear
        // -------------------------------------------------------------------
        [HttpGet("Crear")]
        public IActionResult Crear()
        {
            BannersD bannerss = new() { Id = 0 };
            return View("~/Web/Views/Banners/Create.cshtml", bannerss);
        }

        // -------------------------------------------------------------------
        // POST: /Configuracion/Banners/Crear
        // -------------------------------------------------------------------
        [HttpPost("Crear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(BannersCreateRequest request)
        {
            // Procesar imagen si existe
            request.Image = await _imageStorage.UpdateAsync(
                request.ImageFile,
                request.Image,
                "Banners"
            );

            var result = await _bannersService.CreateAsync(request);
            return HandleResult(result, nameof(Index));
        }

        // -------------------------------------------------------------------
        // GET: /Configuracion/Banners/Editar/{id}
        // -------------------------------------------------------------------
        [HttpGet("Editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var result = await _bannersService.GetByIdAsync(
                new GetByIdRequest<long> { Id = id });

            return HandleResultView(result, "~/Web/Views/Banners/Edit.cshtml");
        }

        // -------------------------------------------------------------------
        // POST: /Configuracion/Banners/Editar
        // -------------------------------------------------------------------
        [HttpPost("Editar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(long id, BannersUpdateRequest request)
        {

            // Procesar imagen si existe
            request.Image = await _imageStorage.UpdateAsync(
                request.ImageFile,
                request.Image,
                "Banners"
            );

            var result = await _bannersService.UpdateAsync(request);
            return HandleResult(result, nameof(Index));
        }

        // -------------------------------------------------------------------
        // GET: /Configuracion/Banners/Eliminar/{id}
        // -------------------------------------------------------------------
        [HttpGet("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result = await _bannersService.GetByIdAsync(
                new GetByIdRequest<long> { Id = id });

            return HandleResultView(result, "~/Web/Views/Banners/Delete.cshtml");
        }

        // -------------------------------------------------------------------
        // POST: /Configuracion/Banners/Eliminar
        // -------------------------------------------------------------------
        [HttpPost("Eliminar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(long id, DeleteRequest request)
        {
            var result = await _bannersService.DeleteAsync(request);
            return HandleResult(result, nameof(Index));
        }

        // -------------------------------------------------------------------
        // ENDPOINT PARA DATATABLES (server-side)
        // -------------------------------------------------------------------
        [HttpPost("ObtenerBannersDataTable")]
        [IgnoreAntiforgeryToken] // DataTables no envía token CSRF
        [SkipModelValidation] // Excluir de la validación automática del filtro
        public async Task<IActionResult> GetBannersDataTable([FromForm] DataTableRequest<BannersD> request)
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
                var result = await _bannersService.GetBannersDataTable(User, request);

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
                _logger.LogError(ex, "Error al obtener banners para DataTables");

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
                        message = "El nombre de el banners es obligatorio."
                    });
                }

                var response = await _bannersService.GetByTituloAsync(id, name);

                if (response == null)
                {
                    return Json(new
                    {
                        result = (object?)null,
                        message = "No se encontró el banners solicitada."
                    });
                }

                return Json(new
                {
                    result = response,
                    message = "Banners obtenida correctamente."
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
                var result = await _bannersService.ToggleEstadoAsync(id);

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