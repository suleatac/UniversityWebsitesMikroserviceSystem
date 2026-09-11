using Microservice.Web.Clients.BannerClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.Banner;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class BannerService : IBannerService
    {

        private readonly IBannerClientServices _bannerClient;
        private readonly ILogger<BannerService> _logger;

        public BannerService(IBannerClientServices bannerClient, ILogger<BannerService> logger)
        {
            _bannerClient = bannerClient ?? throw new ArgumentNullException(nameof(bannerClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }



        public async Task<ServiceResult<BannerDetailVm>> GetBannerByIdAsync(int id)
        {
            _logger.LogInformation("Banner getiriliyor. Id: {Id}", id);
            var response = await _bannerClient.GetBannerByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<BannerDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Banner bulunamadı");
            }

            return ServiceResult<BannerDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<List<GetBannerVm>>> GetBannersAsync(int siteId, int dilId)
        {
            var response = await _bannerClient.GetBannersAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error?.Content is { } content
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(content)
                    : null;

                return ServiceResult<List<GetBannerVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Banner listesi alınamadı");
            }

            return ServiceResult<List<GetBannerVm>>.Success(response.Content ?? new List<GetBannerVm>());
        }
    }
}
