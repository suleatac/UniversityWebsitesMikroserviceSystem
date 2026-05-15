using Microservice.Admin.Clients.BannerClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Banner;
using System.Text.Json;

namespace Microservice.Admin.Services
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

        public async Task<ServiceResult<List<GetBannerVm>>> GetBannersAsync(int siteId, int dilId)
        {
            _logger.LogInformation("Banner listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            var response = await _bannerClient.GetBannersAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetBannerVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Bannerlar alınamadı");
            }

            return ServiceResult<List<GetBannerVm>>.Success(response.Content!);
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

        public async Task<ServiceResult<object>> CreateBannerAsync(CreateBannerVm dto)
        {
            _logger.LogInformation("Yeni banner oluşturuluyor. Başlık: {Title}", dto.Baslik);
            var response = await _bannerClient.CreateBannerAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Banner oluşturulamadı");
            }

            _logger.LogInformation("Banner oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdateBannerAsync(BannerDetailVm dto)
        {
            _logger.LogInformation("Banner güncelleniyor. Id: {Id}", dto.Id);
            var response = await _bannerClient.UpdateBannerAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Banner güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("Banner güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeleteBannerAsync(int id)
        {
            _logger.LogWarning("Banner silme isteği alındı. Id: {Id}", id);
            var response = await _bannerClient.DeleteBannerAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Banner silinemedi");
            }

            _logger.LogInformation("Banner silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<PaginatedResult<GetBannerVm>>> GetBannersPaginatedAsync(int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir)
        {
            _logger.LogInformation("Paginated banner listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            var response = await _bannerClient.GetBannersPaginatedAsync(siteId, dilId, page, pageSize, search, orderBy, orderDir);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}", response.StatusCode, problemDetails?.Title);
                return ServiceResult<PaginatedResult<GetBannerVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated banner listesi alınamadı");
            }

            return ServiceResult<PaginatedResult<GetBannerVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<object>> ReorderBannersAsync(List<ReorderBannerItemVm> items)
        {
            _logger.LogInformation("Banner sıralaması güncelleniyor. Öğe sayısı: {Count}", items?.Count ?? 0);
            var newReorderList = new ReorderBannersCommandListVm {
                Items = items ?? new List<ReorderBannerItemVm>()
            };
            var response = await _bannerClient.ReorderBannersAsync(newReorderList);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Sıralama güncellenemedi");
            }

            return ServiceResult<object>.Success(true);
        }
    }
}
