using Ecommers.Application.DTOs.Requests.Auth;
using Microsoft.AspNetCore.Identity;

namespace Ecommers.Application.Interfaces
{
    public interface IAuthService
    {
        Task<SignInResult> LoginAsync(LoginRequest model);
        Task LogoutAsync();
        //Task<IdentityResult> RegisterAsync(RegisterViewModel model);
        //Task LogoutAsync();
        //Task<string> GeneratePasswordResetTokenAsync(string email);
        //Task<IdentityResult> ResetPasswordAsync(string email, string token, string newPassword);
        //Task<bool> IsEmailConfirmedAsync(string email);
    }
}
