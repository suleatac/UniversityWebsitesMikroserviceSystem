using Microservice.Web.Clients.SikcaSorulanSoruClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.SikcaSorulanSoru;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class SikcaSorulanSoruService : ISikcaSorulanSoruService
    {
        private readonly ISikcaSorulanSoruClientServices _sssClient;
        private readonly ILogger<SikcaSorulanSoruService> _logger;

        public SikcaSorulanSoruService(
            ISikcaSorulanSoruClientServices sssClient,
            ILogger<SikcaSorulanSoruService> logger)
        {
            _sssClient = sssClient ?? throw new ArgumentNullException(nameof(sssClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<SikcaSorulanSoruVm>>> GetSikcaSorulanSorularAsync(int siteId, int dilId)
        {
            var response = await _sssClient.GetSikcaSorulanSorularAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode || response.Content is null)
            {
                var problemDetails = response.Error?.Content is { } content
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(content)
                    : null;

                _logger.LogWarning(
                    "SSS listesi alinamadi. SiteId: {SiteId}, DilId: {DilId}, StatusCode: {StatusCode}",
                    siteId,
                    dilId,
                    response.StatusCode);

                return ServiceResult<List<SikcaSorulanSoruVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Sıkça Sorulan Sorular alınamadı");
            }

            return ServiceResult<List<SikcaSorulanSoruVm>>.Success(response.Content);
        }
    }
}
