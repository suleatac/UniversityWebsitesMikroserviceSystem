using Microservice.Admin.Clients.DuyuruClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Duyuru;
using System.Text.Json;

namespace Microservice.Admin.Services
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

        public async Task<ServiceResult<object>> CreateDuyuruAsync(CreateDuyuruVm dto)
        {
            _logger.LogInformation("Yeni duyuru oluşturuluyor. Başlık: {Title}", dto.Baslik);
            var response = await _duyuruClient.CreateDuyuruAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Duyuru oluşturulamadı");
            }

            _logger.LogInformation("Duyuru oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdateDuyuruAsync(DuyuruDetailVm dto)
        {
            _logger.LogInformation("Duyuru güncelleniyor. Id: {Id}", dto.Id);
            var response = await _duyuruClient.UpdateDuyuruAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Duyuru güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("Duyuru güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeleteDuyuruAsync(int id)
        {
            _logger.LogWarning("Duyuru silme isteği alındı. Id: {Id}", id);
            var response = await _duyuruClient.DeleteDuyuruAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Duyuru silinemedi");
            }

            _logger.LogInformation("Duyuru silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<PaginatedResult<GetDuyuruVm>>> GetDuyurularPaginatedAsync(int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir)
        {
            _logger.LogInformation("Paginated duyuru listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            var response = await _duyuruClient.GetDuyurularPaginatedAsync(siteId, dilId, page, pageSize, search, orderBy, orderDir);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}", response.StatusCode, problemDetails?.Title);
                return ServiceResult<PaginatedResult<GetDuyuruVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated duyuru listesi alınamadı");
            }

            return ServiceResult<PaginatedResult<GetDuyuruVm>>.Success(response.Content!);
        }
    }
}