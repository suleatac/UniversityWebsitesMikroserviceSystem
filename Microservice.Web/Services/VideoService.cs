using Microservice.Web.Clients.VideoClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Content;

namespace Microservice.Web.Services
{
    public sealed class VideoService(IVideoClientServices client) : IVideoService
    {
        public async Task<ServiceResult<ContentDetailVm>> GetVideoByIdAsync(int id)
        {
            var response = await client.GetVideoByIdAsync(id);
            return ContentLookupService.ToResult(response, "Video bulunamadı");
        }
    }
}
