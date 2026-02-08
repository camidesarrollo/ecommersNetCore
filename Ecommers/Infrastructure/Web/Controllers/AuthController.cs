using Ecommers.Application.DTOs.Requests.Auth;
using Ecommers.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ecommers.Infrastructure.Web.Controllers
{
    public class AuthController(
        SignInManager<AspNetUsers> signInManager,
        UserManager<AspNetUsers> userManager,
        ILogger<AuthController> logger) : Controller
    {

        private readonly SignInManager<AspNetUsers> _signInManager = signInManager;
        private readonly UserManager<AspNetUsers> _userManager = userManager;
        private readonly ILogger<AuthController> _logger = logger;

        public IActionResult Login()
        {
            // Si ya está autenticado, redirigir al dashboard
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.AppName = "NutriStore"; // O desde configuración

            return View("~/Web/Views/Auth/Login.cshtml", new LoginRequest { Email = "", Password = "" });
        }

        // POST: Auth/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            ViewBag.AppName = "Tu Tienda";

            if (!ModelState.IsValid)
            {
                return View("~/Web/Views/Auth/Login.cshtml", model);
            }

            try
            {
                // Buscar usuario por email
                var user = await _userManager.FindByEmailAsync(model.Email.ToUpper());

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                    return View("~/Web/Views/Auth/Login.cshtml",model);
                }

                // Verificar si el usuario está activo
                if (!user.IsActive || user.DeletedAt.HasValue)
                {
                    ModelState.AddModelError(string.Empty, "Esta cuenta ha sido desactivada.");
                    return View("~/Web/Views/Auth/Login.cshtml", model);
                }

                // Verificar si el email está confirmado (opcional)
                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError(string.Empty, "Debes confirmar tu email antes de iniciar sesión.");
                    return View("~/Web/Views/Auth/Login.cshtml", model);
                }

                // Intentar iniciar sesión
                var result = await _signInManager.PasswordSignInAsync(
                    user?.UserName ?? "",
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true // Bloquea después de varios intentos fallidos
                );

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Dashboard");
                }


                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Tu cuenta ha sido bloqueada temporalmente por múltiples intentos fallidos.");
                    return View("~/Web/Views/Auth/Login.cshtml", model);
                }

                ModelState.AddModelError(string.Empty, "Credenciales inválidas.");
                return View("~/Web/Views/Auth/Login.cshtml", model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el inicio de sesión");
                ModelState.AddModelError(string.Empty, "Ocurrió un error durante el inicio de sesión. Por favor, inténtalo de nuevo.");
                return View("~/Web/Views/Auth/Login.cshtml", model);
            }
        }
    }
}
