using Ecommers.Application.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
namespace Ecommers.Application.Services
{
    public class FileManagerService(
        IWebHostEnvironment environment,
        ILogger<FileManagerService> logger) : IFileManager
    {
        private readonly IWebHostEnvironment _environment = environment;
        private readonly ILogger<FileManagerService> _logger = logger;
        private const string BASE_PATH = "img";
        private readonly HashSet<string> _allowedExtensions =
    [
        ".jpg", ".jpeg", ".png", ".gif", ".webp", ".svg", ".bmp", ".jfif"
    ];
        private const long MAX_FILE_SIZE = 5 * 1024 * 1024; // 5MB

        public async Task<string> UploadFileAsync(IFormFile file, string subfolder = "")
        {
            try
            {
                if (file == null || file.Length == 0)
                    throw new ArgumentException("El archivo está vacío o es nulo.");

                if (file.Length > MAX_FILE_SIZE)
                    throw new ArgumentException($"El archivo excede el tamaño máximo permitido de {MAX_FILE_SIZE / 1024 / 1024}MB.");

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                    throw new ArgumentException($"Extensión no permitida.");

                // 🔹 SANITIZAR SUBFOLDER
                var safeSubfolder = SanitizeSubfolder(subfolder);

                var basePath = Path.Combine(_environment.WebRootPath, "img");
                var targetDirectory = Path.Combine(basePath, safeSubfolder);

                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);

                // =====================
                // SVG (sin conversión)
                // =====================
                if (extension == ".svg")
                {
                    var fileName = $"{Guid.NewGuid()}.svg";
                    var physicalPath = Path.Combine(targetDirectory, fileName);

                    using (var stream = new FileStream(physicalPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return $"/img/{Path.Combine(safeSubfolder, fileName).Replace("\\", "/")}";
                }

                // =====================
                // IMÁGENES → WEBP
                // =====================
                var webpName = $"{Guid.NewGuid()}.webp";
                var webpPath = Path.Combine(targetDirectory, webpName);

                using var memoryStream = new MemoryStream();
                await file.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                using (var image = await Image.LoadAsync(memoryStream))
                {
                    var encoder = new WebpEncoder
                    {
                        Quality = 80,
                        FileFormat = WebpFileFormatType.Lossy
                    };

                    await image.SaveAsync(webpPath, encoder);
                }

                return $"/img/{Path.Combine(safeSubfolder, webpName).Replace("\\", "/")}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir archivo");
                throw;
            }
        }

        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fileUrl))
                    return false;

                var physicalPath = GetPhysicalPath(fileUrl);

                if (!File.Exists(physicalPath))
                {
                    _logger.LogWarning("Intento de eliminar archivo inexistente: {Path}", physicalPath);
                    return false;
                }

                // Validar que el archivo esté dentro de wwwroot/img
                if (!IsPathSafe(physicalPath))
                {
                    _logger.LogError("Intento de eliminar archivo fuera del directorio permitido: {Path}", physicalPath);
                    throw new UnauthorizedAccessException("Ruta de archivo no permitida.");
                }

                await Task.Run(() => File.Delete(physicalPath));

                _logger.LogInformation("Archivo eliminado exitosamente: {Url}", fileUrl);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar archivo: {Url}", fileUrl);
                throw;
            }
        }

        public async Task<string> UpdateFileAsync(IFormFile newFile, string oldFileUrl, string subfolder = "")
        {
            try
            {
                // Subir nuevo archivo
                var newUrl = await UploadFileAsync(newFile, subfolder);

                // Eliminar archivo anterior si existe
                if (!string.IsNullOrWhiteSpace(oldFileUrl))
                {
                    try
                    {
                        await DeleteFileAsync(oldFileUrl);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "No se pudo eliminar el archivo anterior: {Url}", oldFileUrl);
                        // No lanzar excepción, el nuevo archivo ya fue subido
                    }
                }

                return newUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar archivo");
                throw;
            }
        }

        public bool FileExists(string fileUrl)
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return false;

            var physicalPath = GetPhysicalPath(fileUrl);
            return File.Exists(physicalPath);
        }

        private string GetPhysicalPath(string fileUrl)
        {
            // Remover barra inicial si existe
            var relativePath = fileUrl.TrimStart('/').Replace("/", "\\");
            return Path.Combine(_environment.WebRootPath, relativePath);
        }

        private bool IsPathSafe(string physicalPath)
        {
            var basePath = Path.Combine(_environment.WebRootPath, BASE_PATH);
            var fullBasePath = Path.GetFullPath(basePath);
            var fullFilePath = Path.GetFullPath(physicalPath);

            return fullFilePath.StartsWith(fullBasePath, StringComparison.OrdinalIgnoreCase);
        }

        private static string SanitizeSubfolder(string subfolder)
        {
            if (string.IsNullOrWhiteSpace(subfolder))
                return string.Empty;

            // Normalizar separadores a '/'
            subfolder = subfolder.Replace("\\", "/");

            var invalidChars = Path.GetInvalidFileNameChars();

            var safeParts = subfolder
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .Where(p => p != "." && p != "..") // evita traversal
                .Select(part =>
                    string.Concat(part.Select(c => invalidChars.Contains(c) ? '_' : c))
                );

            return Path.Combine(safeParts.ToArray());
        }
        private static async Task ConvertToWebpAsync(Stream input, string outputPath)
        {
            using var image = await Image.LoadAsync(input);

            // Opcional: puedes ajustar tamaño o filtros aquí

            var encoder = new WebpEncoder
            {
                Quality = 80, // buena compresión sin perder calidad visible
                FileFormat = WebpFileFormatType.Lossy
            };

            await image.SaveAsync(outputPath, encoder);
        }
    }
}
