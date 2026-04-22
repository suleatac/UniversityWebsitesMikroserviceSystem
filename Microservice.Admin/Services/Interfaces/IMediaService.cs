namespace Microservice.Admin.Services.Interfaces
{
    public interface IMediaService
    {
        Task<string> UploadAsync(IFormFile file, int siteId, string module);
    }
}
