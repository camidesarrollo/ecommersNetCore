using System.Xml.Linq;
using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Ecommers.Application.Interfaces;
using Ecommers.Application.DTOs.Requests.Servicios;

namespace Ecommers.Web.Controllers
{
    [Route("Gestion/[controller]")]
    [ServiceFilter(typeof(ValidateModelFilter))]
    public class ServiciosController(
        IServicio serviciosService,
        IImageStorage imageStorage,
        IMapper mapper,
        ILogger<ServiciosController> logger) : BaseController
    {
        private readonly IServicio _servicioService = serviciosService;
        private readonly IImageStorage _imageStorage = imageStorage;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<ServiciosController> _logger = logger;

        // -------------------------------------------------------------------
        // GET: Vista principal
        // -------------------------------------------------------------------
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Web/Views/Servicios/Index.cshtml");
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/Servicios/Detalle/{id}
        // -------------------------------------------------------------------
        [HttpGet("Detalle/{id}")]
        public async Task<IActionResult> Detalle(int id)
        {
            var result = await _servicioService.GetByIdAsync(
                new GetByIdRequest<long> { Id = id });

            return HandleResultView(result, "~/Web/Views/Servicios/Details.cshtml");
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/Servicios/Crear
        // -------------------------------------------------------------------
        [HttpGet("Crear")]
        public IActionResult Crear()
        {
            ServiciosD servicios = new() { Id = 0 };
            return View("~/Web/Views/Servicios/Create.cshtml", servicios);
        }

        // -------------------------------------------------------------------
        // POST: /Gestion/Servicios/Crear
        // -------------------------------------------------------------------
        [HttpPost("Crear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ServiciosCreateRequest request)
        {
            // Procesar imagen si existe
            request.Image = await _imageStorage.UpdateAsync(
                request.ImageFile,
                request.Image,
                "Servicios"
            );

            var result = await _servicioService.CreateAsync(request);
            return HandleResult(result, nameof(Index));
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/Servicios/Editar/{id}
        // -------------------------------------------------------------------
        [HttpGet("Editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var result = await _servicioService.GetByIdAsync(
                new GetByIdRequest<long> { Id = id });

            return HandleResultView(result, "~/Web/Views/Servicios/Edit.cshtml");
        }

        // -------------------------------------------------------------------
        // POST: /Gestion/Servicios/Editar
        // -------------------------------------------------------------------
        [HttpPost("Editar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(long id, ServiciosUpdateRequest request)
        {

            // Procesar imagen si existe
            request.Image = await _imageStorage.UpdateAsync(
                request.ImageFile,
                request.Image,
                "Servicios"
            );

            var result = await _servicioService.UpdateAsync(request);
            return HandleResult(result, nameof(Index));
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/Servicios/Eliminar/{id}
        // -------------------------------------------------------------------
        [HttpGet("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result = await _servicioService.GetByIdAsync(
                new GetByIdRequest<long> { Id = id });

            return HandleResultView(result, "~/Web/Views/Servicios/Delete.cshtml");
        }

        // -------------------------------------------------------------------
        // POST: /Gestion/Servicios/Eliminar
        // -------------------------------------------------------------------
        [HttpPost("Eliminar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(long id, DeleteRequest request)
        {
            var result = await _servicioService.DeleteAsync(request);
            return HandleResult(result, nameof(Index));
        }

        // -------------------------------------------------------------------
        // ENDPOINT PARA DATATABLES (server-side)
        // -------------------------------------------------------------------
        [HttpPost("ObtenerServiciosDataTable")]
        [IgnoreAntiforgeryToken] // DataTables no envía token CSRF
        [SkipModelValidation] // Excluir de la validación automática del filtro
        public async Task<IActionResult> GetServiciosDataTable([FromForm] DataTableRequest<ServiciosD> request)
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
                var result = await _servicioService.GetServicioDataTable(User, request);

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
                _logger.LogError(ex, "Error al obtener servicios para DataTables");

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
                        message = "El nombre de el servicio es obligatorio."
                    });
                }

                var response = await _servicioService.GetByNameAsync(id, name);

                if (response == null)
                {
                    return Json(new
                    {
                        result = (object?)null,
                        message = "No se encontró el servicio solicitada."
                    });
                }

                return Json(new
                {
                    result = response,
                    message = "Servicio obtenido correctamente."
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


    }
}