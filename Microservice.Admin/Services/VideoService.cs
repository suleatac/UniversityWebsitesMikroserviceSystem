using Microservice.Admin.Clients.VideoClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Video;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class VideoService : IVideoService
    {
        private readonly IVideoClientServices _videoClient;
        private readonly ILogger<VideoService> _logger;

        public VideoService(IVideoClientServices videoClient, ILogger<VideoService> logger)
        {
            _videoClient = videoClient ?? throw new ArgumentNullException(nameof(videoClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetVideoVm>>> GetVideosAsync(int siteId, int dilId)
        {
            _logger.LogInformation("Video listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            var response = await _videoClient.GetVideosAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetVideoVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Videolar alınamadı");
            }

            return ServiceResult<List<GetVideoVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<VideoDetailVm>> GetVideoByIdAsync(int id)
        {
            _logger.LogInformation("Video getiriliyor. Id: {Id}", id);
            var response = await _videoClient.GetVideoByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<VideoDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Video bulunamadı");
            }

            return ServiceResult<VideoDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<object>> CreateVideoAsync(CreateVideoVm dto)
        {
            _logger.LogInformation("Yeni video oluşturuluyor. Başlık: {Title}", dto.Baslik);
            var response = await _videoClient.CreateVideoAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Video oluşturulamadı");
            }

            _logger.LogInformation("Video oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdateVideoAsync(VideoDetailVm dto)
        {
            _logger.LogInformation("Video güncelleniyor. Id: {Id}", dto.Id);
            var response = await _videoClient.UpdateVideoAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Video güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("Video güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeleteVideoAsync(int id)
        {
            _logger.LogWarning("Video silme isteği alındı. Id: {Id}", id);
            var response = await _videoClient.DeleteVideoAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Video silinemedi");
            }

            _logger.LogInformation("Video silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<PaginatedResult<GetVideoVm>>> GetVideosPaginatedAsync(int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir)
        {
            _logger.LogInformation("Paginated video listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            var response = await _videoClient.GetVideosPaginatedAsync(siteId, dilId, page, pageSize, search, orderBy, orderDir);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}", response.StatusCode, problemDetails?.Title);
                return ServiceResult<PaginatedResult<GetVideoVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated video listesi alınamadı");
            }

            return ServiceResult<PaginatedResult<GetVideoVm>>.Success(response.Content!);
        }
    }
}