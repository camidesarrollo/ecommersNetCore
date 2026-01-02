using Ecommers.Application.Interfaces;

namespace Ecommers.Infrastructure.Files
{
    public class ImageStorageService(IFileManager fileManager) : IImageStorage
    {
        private readonly IFileManager _fileManager = fileManager;

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
