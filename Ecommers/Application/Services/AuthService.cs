using Ecommers.Application.DTOs.Requests.Auth;
using Ecommers.Application.Interfaces;
using Ecommers.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;

namespace Ecommers.Application.Services
{
    public class AuthService(
        UserManager<AspNetUsers> userManager,
        SignInManager<AspNetUsers> signInManager,
        ILogger<AuthService> logger) : IAuthService
    {
        private readonly UserManager<AspNetUsers> _userManager = userManager;
        private readonly SignInManager<AspNetUsers> _signInManager = signInManager;
        private readonly ILogger<AuthService> _logger = logger;

        public async Task<SignInResult> LoginAsync(LoginRequest model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    _logger.LogWarning($"Intento de inicio de sesión con email no registrado: {model.Email}");
                    return SignInResult.Failed;
                }

                if (!user.IsActive || user.DeletedAt.HasValue)
                {
                    _logger.LogWarning($"Intento de inicio de sesión con cuenta desactivada: {model.Email}");
                    return SignInResult.NotAllowed;
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName ?? "",
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true
                );

                if (result.Succeeded)
                {
                    _logger.LogInformation($"Usuario {user.Email} inició sesión exitosamente.");
                }
                else if (result.IsLockedOut)
                {
                    _logger.LogWarning($"Cuenta bloqueada: {user.Email}");
                }
                else
                {
                    _logger.LogWarning($"Intento de inicio de sesión fallido para: {user.Email}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error durante el inicio de sesión para: {model.Email}");
                throw;
            }
        }
    }
}
