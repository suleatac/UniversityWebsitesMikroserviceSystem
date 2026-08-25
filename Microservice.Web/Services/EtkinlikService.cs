using Microservice.Web.Clients.EtkinlikClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Content;

namespace Microservice.Web.Services
{
    public sealed class EtkinlikService(IEtkinlikClientServices client) : IEtkinlikService
    {
        public async Task<ServiceResult<ContentDetailVm>> GetEtkinlikByIdAsync(int id)
        {
            var response = await client.GetEtkinlikByIdAsync(id);
            return ContentLookupService.ToResult(response, "Etkinlik bulunamadı");
        }
    }
}
