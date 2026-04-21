namespace Microservice.Admin.Services.Contracts
{
    public interface IMediaService
    {
        Task<string> UploadAsync(IFormFile file, int siteId, string module);
    }
}
