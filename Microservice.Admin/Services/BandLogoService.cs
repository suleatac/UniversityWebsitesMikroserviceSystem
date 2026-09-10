using Microservice.Admin.Clients.BandLogoClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.BandLogo;
using System.Text.Json;

namespace Microservice.Admin.Services
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

        // CREATE
        public async Task<ServiceResult<object>> CreateBandLogoAsync(CreateBandLogoVm dto)
        {
            _logger.LogInformation("Yeni band logosu oluşturuluyor. Ad: {Ad}", dto.Ad);
            var response = await _bandLogoClient.CreateBandLogoAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Band logosu oluşturulamadı");
            }

            _logger.LogInformation("Band logosu oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        // UPDATE
        public async Task<ServiceResult<object>> UpdateBandLogoAsync(BandLogoDetailVm dto)
        {
            _logger.LogInformation("Band logosu güncelleniyor. Id: {Id}", dto.Id);
            var response = await _bandLogoClient.UpdateBandLogoAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Band logosu güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("Band logosu güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        // DELETE
        public async Task<ServiceResult<object>> DeleteBandLogoAsync(int id)
        {
            _logger.LogWarning("Band logosu silme isteği alındı. Id: {Id}", id);
            var response = await _bandLogoClient.DeleteBandLogoAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Band logosu silinemedi");
            }

            _logger.LogInformation("Band logosu silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }

        // PAGINATED LIST
        public async Task<ServiceResult<PaginatedResult<GetBandLogoVm>>> GetBandLogosPaginatedAsync(int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir)
        {
            _logger.LogInformation("Paginated band logosu listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            var response = await _bandLogoClient.GetBandLogosPaginatedAsync(siteId, dilId, page, pageSize, search, orderBy, orderDir);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}", response.StatusCode, problemDetails?.Title);
                return ServiceResult<PaginatedResult<GetBandLogoVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated band logosu listesi alınamadı");
            }

            return ServiceResult<PaginatedResult<GetBandLogoVm>>.Success(response.Content!);
        }
    }
}
