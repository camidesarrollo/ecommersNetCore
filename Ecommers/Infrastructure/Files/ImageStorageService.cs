using Ecommers.Application.Interfaces;

namespace Ecommers.Infrastructure.Files
{
    public class ImageStorageService(IFileManager fileManager) : IImageStorage
    {
        private readonly IFileManager _fileManager = fileManager;

        /// <summary>
        /// Actualiza una imagen: sube la nueva y elimina la anterior
        /// </summary>
        public async Task<string?> UpdateAsync(
            IFormFile? file,
            string? currentImage,
            string folder)
        {
            if (file == null) 
                return currentImage;

            return await _fileManager.UpdateFileAsync(
                file,
                currentImage ?? "",
                folder
            );
        }

        /// <summary>
        /// Elimina una imagen del servidor
        /// </summary>
        public async Task<bool> DeleteAsync(string? imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return false;

            return await _fileManager.DeleteFileAsync(imageUrl);
        }

        /// <summary>
        /// Sube una nueva imagen sin eliminar ninguna anterior
        /// </summary>
        public async Task<string> UploadAsync(IFormFile file, string folder)
        {
            ArgumentNullException.ThrowIfNull(file);

            return await _fileManager.UploadFileAsync(file, folder);
        }
    }
}
