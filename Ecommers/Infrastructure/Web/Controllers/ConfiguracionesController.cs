using AutoMapper;
using Ecommers.Application.DTOs.Common;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Ecommers.Infrastructure.Web.Controllers
{
    [Route("Configuracion/[controller]")]
    public class ConfiguracionesController(IConfiguracion configuracionService, IMapper mapper, IFileManager fileManager, ICompositeViewEngine viewEngine) : BaseController(viewEngine)
    {
        private readonly IConfiguracion _configuracionService = configuracionService;
        private readonly IFileManager _fileManager = fileManager;
        private readonly IMapper _mapper = mapper;

        // -------------------------------------------------------------------
        // GET: /Configuracion/Configuraciones
        // Vista principal con listado
        // -------------------------------------------------------------------
        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var configuraciones = await _configuracionService.GetAllAsync();
                return View("~/Web/Views/Configuracion/Index.cshtml", configuraciones);
            }
            catch (Exception)
            {
                return View(new List<ConfiguracionesD>());
            }
        }


        // -------------------------------------------------------------------
        // GET: /Configuracion/Configuraciones/Editar/{id}
        // Vista formulario de edición
        // -------------------------------------------------------------------
        [HttpGet("Editar/{id}")]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var configuracion = await _configuracionService.GetByIdAsync(new GetByIdRequest<int> { Id = id });

                if (configuracion == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View("~/Web/Views/Configuracion/Edit.cshtml", configuracion);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // -------------------------------------------------------------------
        // POST: /Configuracion/Configuraciones/Editar/{id}
        // Procesar edición
        // -------------------------------------------------------------------
        [HttpPost("Editar/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, ConfiguracionUpdateRequest request)
        {
            try
            {
                if (id != request.Id)
                {

                    ConfiguracionesD configuracion = new() { Id = id };
                    _mapper.Map(request, configuracion); // Se actualizan propiedades
                    return View("~/Web/Views/Configuracion/Edit.cshtml", configuracion);
                }

                if (!ModelState.IsValid)
                {
                    // Obtener todos los mensajes de error
                    var errores = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    // Guardarlos en TempData

                    TempData["mensaje"] = string.Join("<br>", errores);
                    ConfiguracionesD configuracion = new() { Id = id };
                    _mapper.Map(request, configuracion); // Se actualizan propiedades
                    return View("~/Web/Views/Configuracion/Edit.cshtml", configuracion);
                }
                string? newImageUrl = null;
                if (request.FaviconFile != null)
                {
                    newImageUrl = await _fileManager.UpdateFileAsync(
                        request.FaviconFile,
                        request.Favicon ?? "",
                        "Configuraciones"
                    );

                    request.Favicon = newImageUrl;
                }

                if (request.BannerFile != null)
                {
                    newImageUrl = await _fileManager.UpdateFileAsync(
                        request.BannerFile,
                        request.BannerPrincipal ?? "",
                        "Configuraciones"
                    );

                    request.BannerPrincipal = newImageUrl;
                }

                if (request.LogoFile != null)
                {
                    newImageUrl = await _fileManager.UpdateFileAsync(
                        request.LogoFile,
                        request.Logo ?? "",
                        "Configuraciones"
                    );

                    request.Logo = newImageUrl;
                }


                await _configuracionService.UpdateAsync(request);
                TempData["mensaje"] = "Configuración actualizada exitosamente";
                TempData["estado"] = "Exito";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                TempData["mensaje"] = e.Message;
                TempData["estado"] = "Error";
                ConfiguracionesD configuracion = new() { Id = id };
                _mapper.Map(request, configuracion); // Se actualizan propiedades
                return View("~/Web/Views/Configuracion/Edit.cshtml", configuracion);
            }
        }

        // -------------------------------------------------------------------
        // GET: /Configuracion/Configuraciones/Detalle/{id}
        // Vista de detalles (solo lectura)
        // -------------------------------------------------------------------
        [HttpGet("Detalle/{id}")]
        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var configuracion = await _configuracionService.GetByIdAsync(new GetByIdRequest<int> { Id = id });

                if (configuracion == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                return View("~/Web/Views/Configuracion/Details.cshtml", configuracion);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}