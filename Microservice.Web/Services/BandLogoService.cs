using Microservice.Web.Clients.BandLogoClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels;
using Microservice.Web.ViewModels.BandLogo;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class BandLogoService : IBandLogoService
    {
        private readonly IBandLogoClientServices _bandLogoClient;
        private readonly ILogger<BandLogoService> _logger;

        public BandLogoService(IBandLogoClientServices bandLogoClient, ILogger<BandLogoService> logger)
        {
            _bandLogoClient = bandLogoClient ?? throw new ArgumentNullException(nameof(bandLogoClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // LIST
        public async Task<ServiceResult<List<GetBandLogoVm>>> GetBandLogosAsync(int siteId, int dilId)
        {
            _logger.LogInformation("BandLogo listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            var response = await _bandLogoClient.GetBandLogosAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetBandLogoVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Band logoları alınamadı");
            }

            return ServiceResult<List<GetBandLogoVm>>.Success(response.Content!);
        }

        // GET BY ID
        public async Task<ServiceResult<BandLogoDetailVm>> GetBandLogoByIdAsync(int id)
        {
            _logger.LogInformation("BandLogo getiriliyor. Id: {Id}", id);
            var response = await _bandLogoClient.GetBandLogoByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<BandLogoDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Band logosu bulunamadı");
            }

            return ServiceResult<BandLogoDetailVm>.Success(response.Content!);
        }

   }
}
