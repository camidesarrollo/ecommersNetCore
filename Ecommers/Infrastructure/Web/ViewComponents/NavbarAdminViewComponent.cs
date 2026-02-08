using System.Security.Claims;
using Ecommers.Application.Interfaces;
using Ecommers.Infrastructure.Persistence.Entities;
using Ecommers.Web.Models.Shared.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Infrastructure.Web.ViewComponents
{
    public class NavbarAdminViewComponent(UserManager<AspNetUsers> userManager) : ViewComponent
    {
        private readonly UserManager<AspNetUsers> _userManager = userManager;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new NavbarAdminViewModel();

            //// Aquí puedes obtener los datos del usuario actual
            //// Por ejemplo, desde el HttpContext
            //if (User.Identity.IsAuthenticated)
            //{
            //    var userName = User.Identity.Name ?? "Administrador";
            //    var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value
            //        ?? "admin@example.com";

            //    model.SetUserInfo(userName, userEmail);
            //}

            // Aquí puedes cargar las notificaciones desde la base de datos
            // model.Notifications = _notificationService.GetUnreadNotifications(userId);
            // model.NotificationCount = model.Notifications.Count(n => !n.Read);

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

            model.LoggedUser = loggedUser;

            return View("~/Web/Views/Shared/Components/NavbarAdmin/Default.cshtml", model);
        }
    }
}