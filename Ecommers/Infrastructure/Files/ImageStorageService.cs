using Ecommers.Application.Interfaces;

namespace Ecommers.Infrastructure.Files
{
    public class ImageStorageService(IFileManagerService fileManager) : IImageStorageService
    {
        private readonly IFileManagerService _fileManager = fileManager;

        public async Task<string?> UpdateAsync(
            IFormFile? file,
            string? currentImage,
            string folder)
        {
            if (file == null) return currentImage;

            return await _fileManager.UpdateFileAsync(
                file,
                currentImage ?? "",
                folder
            );
        }
    }
}
