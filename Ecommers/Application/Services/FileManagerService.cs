using Ecommers.Application.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
namespace Ecommers.Application.Services
{
    public class FileManagerService(
        IWebHostEnvironment environment,
        ILogger<FileManagerService> logger) : IFileManagerService
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
                // Validaciones
                if (file == null || file.Length == 0)
                    throw new ArgumentException("El archivo está vacío o es nulo.");

                if (file.Length > MAX_FILE_SIZE)
                    throw new ArgumentException($"El archivo excede el tamaño máximo permitido de {MAX_FILE_SIZE / 1024 / 1024}MB.");

                var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!_allowedExtensions.Contains(extension))
                    throw new ArgumentException($"Extensión de archivo no permitida. Permitidas: {string.Join(", ", _allowedExtensions)}");

                // Sanitizar subfolder
                subfolder = SanitizeSubfolder(subfolder);

                // ================================
                // SVG: se copia tal cual, sin convertir
                // ================================
                if (extension == ".svg")
                {
                    var svgName = $"{Guid.NewGuid()}.svg";
                    var svgRelative = Path.Combine(BASE_PATH, subfolder, svgName);
                    var svgPhysical = Path.Combine(_environment.WebRootPath, svgRelative);

                    var dir = Path.GetDirectoryName(svgPhysical);
                    if (!Directory.Exists(dir))
                        Directory.CreateDirectory(dir!);

                    using (var stream = new FileStream(svgPhysical, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var urlSvg = $"/{svgRelative.Replace("\\", "/")}";
                    _logger.LogInformation("SVG subido sin conversión: {Url}", urlSvg);
                    return urlSvg;
                }

                // ================================
                // Resto de imágenes → convertir a WEBP
                // ================================
                var fileName = $"{Guid.NewGuid()}.webp"; // SIEMPRE webp
                var relativePath = Path.Combine(BASE_PATH, subfolder, fileName);
                var physicalPath = Path.Combine(_environment.WebRootPath, relativePath);

                // Crear carpeta
                var directory = Path.GetDirectoryName(physicalPath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory!);

                // Convertir a WebP
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

                    await image.SaveAsync(physicalPath, encoder);
                }

                // Retornar URL web
                var url = $"/{relativePath.Replace("\\", "/")}";
                _logger.LogInformation("Imagen convertida y subida como WebP: {Url}", url);

                return url;
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

            // Remover caracteres peligrosos
            var invalidChars = Path.GetInvalidPathChars()
                .Concat(['/', '\\', ':', '*', '?', '"', '<', '>', '|'])
                .ToArray();

            subfolder = string.Join("_", subfolder.Split(invalidChars, StringSplitOptions.RemoveEmptyEntries));

            // Evitar ataques de path traversal
            subfolder = subfolder.Replace("..", "").Trim();

            return subfolder;
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
