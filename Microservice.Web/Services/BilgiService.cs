using Microservice.Web.Clients.BilgiClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Content;

namespace Microservice.Web.Services
{
    public sealed class BilgiService(IBilgiClientServices client) : IBilgiService
    {
        public async Task<ServiceResult<ContentDetailVm>> GetBilgiByIdAsync(int id)
        {
            var response = await client.GetBilgiByIdAsync(id);
            return ContentLookupService.ToResult(response, "Bilgi bulunamadı");
        }
    }
}
