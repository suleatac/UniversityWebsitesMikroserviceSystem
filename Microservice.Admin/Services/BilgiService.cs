using Microservice.Admin.Clients.BilgiClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Bilgi;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class BilgiService : IBilgiService
    {
        private readonly IBilgiClientServices _bilgiClient;
        private readonly ILogger<BilgiService> _logger;

        public BilgiService(IBilgiClientServices bilgiClient, ILogger<BilgiService> logger)
        {
            _bilgiClient = bilgiClient ?? throw new ArgumentNullException(nameof(bilgiClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetBilgiVm>>> GetBilgilerAsync(int siteId, int dilId)
        {
            _logger.LogInformation("Bilgi listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            var response = await _bilgiClient.GetBilgilerAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetBilgiVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Bilgiler alınamadı");
            }

            return ServiceResult<List<GetBilgiVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<BilgiDetailVm>> GetBilgiByIdAsync(int id)
        {
            _logger.LogInformation("Bilgi getiriliyor. Id: {Id}", id);
            var response = await _bilgiClient.GetBilgiByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<BilgiDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Bilgi bulunamadı");
            }

            return ServiceResult<BilgiDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<object>> CreateBilgiAsync(CreateBilgiVm dto)
        {
            _logger.LogInformation("Yeni bilgi oluşturuluyor. Başlık: {Title}", dto.Baslik);
            var response = await _bilgiClient.CreateBilgiAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Bilgi oluşturulamadı");
            }

            _logger.LogInformation("Bilgi oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdateBilgiAsync(BilgiDetailVm dto)
        {
            _logger.LogInformation("Bilgi güncelleniyor. Id: {Id}", dto.Id);
            var response = await _bilgiClient.UpdateBilgiAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Bilgi güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("Bilgi güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeleteBilgiAsync(int id)
        {
            _logger.LogWarning("Bilgi silme isteği alındı. Id: {Id}", id);
            var response = await _bilgiClient.DeleteBilgiAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Bilgi silinemedi");
            }

            _logger.LogInformation("Bilgi silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<PaginatedResult<GetBilgiVm>>> GetBilgilerPaginatedAsync(int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir)
        {
            _logger.LogInformation("Paginated bilgi listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            var response = await _bilgiClient.GetBilgilerPaginatedAsync(siteId, dilId, page, pageSize, search, orderBy, orderDir);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}", response.StatusCode, problemDetails?.Title);
                return ServiceResult<PaginatedResult<GetBilgiVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated bilgi listesi alınamadı");
            }

            return ServiceResult<PaginatedResult<GetBilgiVm>>.Success(response.Content!);
        }
    }
}