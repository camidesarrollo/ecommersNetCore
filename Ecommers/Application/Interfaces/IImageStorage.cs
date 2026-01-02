namespace Ecommers.Application.Interfaces
{
    public interface IImageStorage
    {
        Task<string?> UpdateAsync(IFormFile? file, string? currentImage, string folder);
    }
}
