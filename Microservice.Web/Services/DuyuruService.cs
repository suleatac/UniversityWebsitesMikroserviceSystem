using Microservice.Web.Clients.DuyuruClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Duyuru;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class DuyuruService : IDuyuruService
    {
        private readonly IDuyuruClientServices _duyuruClient;
        private readonly ILogger<DuyuruService> _logger;

        public DuyuruService(IDuyuruClientServices duyuruClient, ILogger<DuyuruService> logger)
        {
            _duyuruClient = duyuruClient ?? throw new ArgumentNullException(nameof(duyuruClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetDuyuruVm>>> GetDuyurularAsync(int siteId, int dilId)
        {
            _logger.LogInformation("Duyuru listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            var response = await _duyuruClient.GetDuyurularAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetDuyuruVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Duyurular alınamadı");
            }

            return ServiceResult<List<GetDuyuruVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<DuyuruDetailVm>> GetDuyuruByIdAsync(int id)
        {
            _logger.LogInformation("Duyuru getiriliyor. Id: {Id}", id);
            var response = await _duyuruClient.GetDuyuruByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<DuyuruDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Duyuru bulunamadı");
            }

            return ServiceResult<DuyuruDetailVm>.Success(response.Content!);
        }

      }
}