using Microservice.Web.Clients.BannerClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Content;

namespace Microservice.Web.Services
{
    public sealed class BannerService(IBannerClientServices client) : IBannerService
    {
        public async Task<ServiceResult<ContentDetailVm>> GetBannerByIdAsync(int id)
        {
            var response = await client.GetBannerByIdAsync(id);
            return ContentLookupService.ToResult(response, "Banner bulunamadı");
        }
    }
}
