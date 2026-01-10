namespace Ecommers.Application.Interfaces
{
    public interface IImageStorage
    {
        Task<string?> UpdateAsync(IFormFile? file, string? currentImage, string folder);
        Task<bool> DeleteAsync(string? imageUrl);
        Task<string> UploadAsync(IFormFile file, string folder);
    }
}