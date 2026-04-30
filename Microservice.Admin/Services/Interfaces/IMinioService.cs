namespace Microservice.Admin.Services.Interfaces
{
    public interface IMinioService
    {
        Task<List<string>> GetFilesAsync(string prefix);
        Task UploadAsync(string prefix, IFormFile file);
        Task DeleteAsync(string objectName);
        public string GetFileUrl(string objectName);
    }
}
