namespace Ecommers.Application.Interfaces
{
    public interface IFileManager
    {
        Task<string> UploadFileAsync(IFormFile file, string subfolder = "");
        Task<bool> DeleteFileAsync(string fileUrl);
        Task<string> UpdateFileAsync(IFormFile newFile, string oldFileUrl, string subfolder = "");
        bool FileExists(string fileUrl);
    }
}
