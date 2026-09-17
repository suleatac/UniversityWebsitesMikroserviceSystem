using Microservice.Web.Clients.IletisimClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Iletisim;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class IletisimService : IIletisimService
    {
        private readonly IIletisimClientServices _iletisimClient;
        private readonly ILogger<IletisimService> _logger;

        public IletisimService(IIletisimClientServices iletisimClient, ILogger<IletisimService> logger)
        {
            _iletisimClient = iletisimClient ?? throw new ArgumentNullException(nameof(iletisimClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult> GonderAsync(IletisimMesajiVm model)
        {
            _logger.LogInformation(
                "Iletisim mesaji Site API'ye gonderiliyor. SiteId: {SiteId}, Gonderen: {Eposta}",
                model.SiteId, model.Eposta);

            var response = await _iletisimClient.GonderAsync(model);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode, problemDetails?.Title, problemDetails?.Detail);

                return ServiceResult.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Mesaj gönderilemedi. Lütfen tekrar deneyin.");
            }

            _logger.LogInformation("Iletisim mesaji Site API tarafina iletildi.");
            return ServiceResult.Success();
        }
    }
}
