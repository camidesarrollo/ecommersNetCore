using System.Xml.Linq;
using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.DataTables;
using Ecommers.Application.DTOs.Requests.MasterAttributes;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Ecommers.Web.Controllers
{
    [Route("Gestion/[controller]")]
    [ServiceFilter(typeof(ValidateModelFilter))]
    public class MasterAttributesController(
        IMasterAttributes MasterAttributesService,
        IImageStorage imageStorage,
        IMapper mapper,
        ILogger<MasterAttributesController> logger) : BaseController
    {
        private readonly IMasterAttributes _MasterAttributeservice = MasterAttributesService;
        private readonly IImageStorage _imageStorage = imageStorage;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<MasterAttributesController> _logger = logger;

        // -------------------------------------------------------------------
        // GET: Vista principal
        // -------------------------------------------------------------------
        [HttpGet("")]
        public IActionResult Index()
        {
            return View("~/Web/Views/MasterAttributes/Index.cshtml");
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/MasterAttributes/Detalle/{id}
        // -------------------------------------------------------------------
        [HttpGet("Detalle/{id}")]
        public async Task<IActionResult> Detalle(int id)
        {
            var result = await _MasterAttributeservice.GetByIdAsync(
                new GetByIdRequest<long> { Id = id });

            return HandleResultView(result, "~/Web/Views/MasterAttributes/Details.cshtml");
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/MasterAttributes/Crear
        // -------------------------------------------------------------------
        [HttpGet("Crear")]
        public IActionResult Crear()
        {
            MasterAttributesD MasterAttributes = new() { Id = 0 };
            return View("~/Web/Views/MasterAttributes/Create.cshtml", MasterAttributes);
        }

        // -------------------------------------------------------------------
        // POST: /Gestion/MasterAttributes/Crear
        // -------------------------------------------------------------------
        [HttpPost("Crear")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(MasterAttributesCreateRequest request)
        {
    
            var result = await _MasterAttributeservice.CreateAsync(request);
            return HandleResult(result, nameof(Index));
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/MasterAttributes/Editar/{id}
        // -------------------------------------------------------------------
        [HttpGet("Editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            var result = await _MasterAttributeservice.GetByIdAsync(
                new GetByIdRequest<long> { Id = id });

            return HandleResultView(result, "~/Web/Views/MasterAttributes/Edit.cshtml");
        }

        // -------------------------------------------------------------------
        // POST: /Gestion/MasterAttributes/Editar
        // -------------------------------------------------------------------
        [HttpPost("Editar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(long id, MasterAttributesUpdateRequest request)
        {

            var result = await _MasterAttributeservice.UpdateAsync(request);
            return HandleResult(result, nameof(Index));
        }

        // -------------------------------------------------------------------
        // GET: /Gestion/MasterAttributes/Eliminar/{id}
        // -------------------------------------------------------------------
        [HttpGet("Eliminar/{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var result = await _MasterAttributeservice.GetByIdAsync(
                new GetByIdRequest<long> { Id = id });

            return HandleResultView(result, "~/Web/Views/MasterAttributes/Delete.cshtml");
        }

        // -------------------------------------------------------------------
        // POST: /Gestion/MasterAttributes/Eliminar
        // -------------------------------------------------------------------
        [HttpPost("Eliminar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Eliminar(long id, DeleteRequest<long>  request)
        {
            var result = await _MasterAttributeservice.DeleteAsync(request);
            return HandleResult(result, nameof(Index));
        }

        // -------------------------------------------------------------------
        // ENDPOINT PARA DATATABLES (server-side)
        // -------------------------------------------------------------------
        [HttpPost("ObtenerMasterAttributesDataTable")]
        [IgnoreAntiforgeryToken] // DataTables no envía token CSRF
        [SkipModelValidation] // Excluir de la validación automática del filtro
        public async Task<IActionResult> GetMasterAttributesDataTable([FromForm] DataTableRequest<MasterAttributesD> request)
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
                var result = await _MasterAttributeservice.GetMasterAttributesDataTable(User, request);

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
                _logger.LogError(ex, "Error al obtener categorías para DataTables");

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
                        message = "El nombre de el maestro de atributos es obligatorio."
                    });
                }

                var response = await _MasterAttributeservice.GetByNameAsync(id, name);

                if (response == null)
                {
                    return Json(new
                    {
                        result = (object?)null,
                        message = "No se encontró el maestro de atributos solicitada."
                    });
                }

                return Json(new
                {
                    result = response,
                    message = "Maestro de atributos obtenida correctamente."
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