using Microservice.Admin.Clients.EtkinlikClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Etkinlik;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class EtkinlikService : IEtkinlikService
    {
        private readonly IEtkinlikClientServices _etkinlikClient;
        private readonly ILogger<EtkinlikService> _logger;

        public EtkinlikService(IEtkinlikClientServices etkinlikClient, ILogger<EtkinlikService> logger)
        {
            _etkinlikClient = etkinlikClient ?? throw new ArgumentNullException(nameof(etkinlikClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetEtkinlikVm>>> GetEtkinliklerAsync(int siteId, int dilId)
        {
            _logger.LogInformation("Etkinlik listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            var response = await _etkinlikClient.GetEtkinliklerAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetEtkinlikVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Etkinlikler alınamadı");
            }

            return ServiceResult<List<GetEtkinlikVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<EtkinlikDetailVm>> GetEtkinlikByIdAsync(int id)
        {
            _logger.LogInformation("Etkinlik getiriliyor. Id: {Id}", id);
            var response = await _etkinlikClient.GetEtkinlikByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<EtkinlikDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Etkinlik bulunamadı");
            }

            return ServiceResult<EtkinlikDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<object>> CreateEtkinlikAsync(CreateEtkinlikVm dto)
        {
            _logger.LogInformation("Yeni etkinlik oluşturuluyor. Başlık: {Title}", dto.Baslik);
            var response = await _etkinlikClient.CreateEtkinlikAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Etkinlik oluşturulamadı");
            }

            _logger.LogInformation("Etkinlik oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> UpdateEtkinlikAsync(EtkinlikDetailVm dto)
        {
            _logger.LogInformation("Etkinlik güncelleniyor. Id: {Id}", dto.Id);
            var response = await _etkinlikClient.UpdateEtkinlikAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Etkinlik güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("Etkinlik güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeleteEtkinlikAsync(int id)
        {
            _logger.LogWarning("Etkinlik silme isteği alındı. Id: {Id}", id);
            var response = await _etkinlikClient.DeleteEtkinlikAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Etkinlik silinemedi");
            }

            _logger.LogInformation("Etkinlik silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<PaginatedResult<GetEtkinlikVm>>> GetEtkinliklerPaginatedAsync(int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir)
        {
            _logger.LogInformation("Paginated etkinlik listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            var response = await _etkinlikClient.GetEtkinliklerPaginatedAsync(siteId, dilId, page, pageSize, search, orderBy, orderDir);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}", response.StatusCode, problemDetails?.Title);
                return ServiceResult<PaginatedResult<GetEtkinlikVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated etkinlik listesi alınamadı");
            }

            return ServiceResult<PaginatedResult<GetEtkinlikVm>>.Success(response.Content!);
        }
    }
}