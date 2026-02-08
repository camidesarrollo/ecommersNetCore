using System.Security.Claims;
using Ecommers.Application.DTOs.Requests;
using Ecommers.Application.DTOs.Requests.Configuracion;
using Ecommers.Application.Interfaces;
using Ecommers.Domain.Entities;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Web.Models.Shared.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Infrastructure.Web.ViewComponents
{
    public class HeaderViewComponent(IConfiguracion configService, UserManager<AspNetUsers> userManager) : ViewComponent
    {
        private readonly IConfiguracion _configService = configService;
        private readonly UserManager<AspNetUsers> _userManager = userManager;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var configuracionCacheRequest = new ConfiguracionCacheRequest
            {
                LastClientUpdate = DateTime.UtcNow
            };

            var configuracion = await _configService.GetCachedAsync(configuracionCacheRequest);

            // Obtener usuario logueado
            LoggedUser? loggedUser = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                var claimsPrincipal = User as ClaimsPrincipal; // <-- conversión
                var userId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        var roles = await _userManager.GetRolesAsync(user); // <-- obtiene roles

                        loggedUser = new LoggedUser
                        {
                            Id = user.Id,
                            UserName = user.UserName ?? "",
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            IsActive = user.IsActive,
                            Roles = roles
                        };
                    }
                }
            }


            configuracion =  configuracion ?? new ConfiguracionesD
            {
                Id = 0, // o algún valor temporal/por defecto
            };


            var model = new HeaderViewModel
            {
                ConfiguracionesD = configuracion,
                LoggedUser = loggedUser
            };

            ViewData["RequiresHeaderCss"] = true;
            ViewData["RequiresHeaderJs"] = true;

            return View("~/Web/Views/Shared/Components/Header/Default.cshtml", model);
        }
    }
}
