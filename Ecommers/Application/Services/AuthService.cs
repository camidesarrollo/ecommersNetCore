using Ecommers.Application.DTOs.Requests.Auth;
using Ecommers.Application.Interfaces;
using Ecommers.Infrastructure.Persistence.Entities;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.SqlServer.Server;

namespace Ecommers.Application.Services
{
    public class AuthService(
    UserManager<AspNetUsers> userManager,
    SignInManager<AspNetUsers> signInManager,
    RoleManager<AspNetRoles> roleManager,
    ILogger<AuthService> logger) : IAuthService
    {
        private readonly UserManager<AspNetUsers> _userManager = userManager;
        private readonly SignInManager<AspNetUsers> _signInManager = signInManager;
        private readonly RoleManager<AspNetRoles> _roleManager = roleManager;
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

        public async Task<SignInResult> LoginWithGoogleAsync(LoginGoogleRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
                throw new Exception("Token de Google requerido.");

            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new List<string>
            {
                Environment.GetEnvironmentVariable("GOOGLE_CLIENT_ID")
                    ?? throw new Exception("GOOGLE_CLIENT_ID no configurado.")
            }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(
                    request.Token,
                    settings
                );

                if (payload == null)
                    throw new Exception("Token de Google inválido.");

                string email = payload.Email;
                string firstName = payload.GivenName ?? payload.Name;
                string lastName = payload.FamilyName ?? "";

                var user = await _userManager.FindByEmailAsync(email);

                // 🔥 SI NO EXISTE → CREAR
                if (user == null)
                {
                    user = new AspNetUsers
                    {
                        UserName = email,
                        Email = email,
                        EmailConfirmed = true,
                        FirstName = firstName,
                        LastName = lastName,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true,
                        TermsAccepted = true,
                        PrivacyAccepted = true
                    };

                    var createResult = await _userManager.CreateAsync(user);

                    if (!createResult.Succeeded)
                    {
                        _logger.LogError("Error creando usuario Google: {Errors}",
                            string.Join(",", createResult.Errors.Select(e => e.Description)));

                        return SignInResult.Failed;
                    }

                    // ✅ Crear rol si no existe
                    if (!string.IsNullOrEmpty(request.Role))
                    {
                        if (!await _roleManager.RoleExistsAsync(request.Role))
                        {
                            await _roleManager.CreateAsync(new AspNetRoles
                            {
                                Name = request.Role
                            });
                        }

                        await _userManager.AddToRoleAsync(user, request.Role);
                    }

                    _logger.LogInformation("Usuario Google creado: {Email}", email);
                }

                // 🔐 Autenticar usuario
                await _signInManager.SignInAsync(user, isPersistent: true);

                return SignInResult.Success;
            }
            catch (InvalidJwtException)
            {
                throw new Exception("Token de Google inválido.");
            }
        }

        public async Task<IdentityResult> RegisterAsync(RegisterRequest request)
        {
            try
            {
                if (!request.TermsAccepted)
                    throw new Exception("Debe aceptar los términos y condiciones.");

                if (request.Password != request.ConfirmPassword)
                    throw new Exception("Las contraseñas no coinciden.");

                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                    throw new Exception("El email ya está registrado.");

                var user = new AspNetUsers
                {
                    UserName = request.Email,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    EmailConfirmed = false, // 🔒 recomendable validar por email
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    TermsAccepted = request.TermsAccepted,
                    PrivacyAccepted = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                    return result;

                // 🔐 Asignar rol por defecto (NUNCA desde frontend)
                const string defaultRole = "Customer";

                if (!await _roleManager.RoleExistsAsync(defaultRole))
                {
                    await _roleManager.CreateAsync(new AspNetRoles
                    {
                        Name = defaultRole
                    });
                }

                await _userManager.AddToRoleAsync(user, defaultRole);

                _logger.LogInformation("Usuario registrado exitosamente: {Email}", user.Email);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el registro para {Email}", request.Email);
                throw;
            }
        }


        public async Task LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("Usuario cerró sesión correctamente.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el cierre de sesión.");
                throw;
            }
        }
    }
}
