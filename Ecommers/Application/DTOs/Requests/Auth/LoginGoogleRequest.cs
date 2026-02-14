namespace Ecommers.Application.DTOs.Requests.Auth
{
    public class LoginGoogleRequest
    {
        public string Token { get; set; }
        public string DeviceInfo { get; set; }
        public string Role { get; set; } // 👈 nuevo
    }
}
