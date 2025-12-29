namespace Ecommers.Application.Interfaces
{
    public interface IImageStorageService
    {
        Task<string?> UpdateAsync(IFormFile? file, string? currentImage, string folder);
    }
}
