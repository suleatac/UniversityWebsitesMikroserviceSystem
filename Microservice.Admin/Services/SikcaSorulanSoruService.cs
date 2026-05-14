using Microservice.Admin.Clients.SikcaSorulanSoruClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.SikcaSorulanSoru;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class SikcaSorulanSoruService : ISikcaSorulanSoruService
    {
        private readonly ISikcaSorulanSoruClientServices _sikcaSorulanSoruClient;
        private readonly ILogger<SikcaSorulanSoruService> _logger;

        public SikcaSorulanSoruService(ISikcaSorulanSoruClientServices sikcaSorulanSoruClient, ILogger<SikcaSorulanSoruService> logger)
        {
            _sikcaSorulanSoruClient = sikcaSorulanSoruClient ?? throw new ArgumentNullException(nameof(sikcaSorulanSoruClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetSikcaSorulanSoruVm>>> GetSikcaSorulanSorularAsync(int siteId, int dilId)
        {
            _logger.LogInformation("SSS listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            var response = await _sikcaSorulanSoruClient.GetSikcaSorulanSorularAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetSikcaSorulanSoruVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "SSS alınamadı");
            }

            return ServiceResult<List<GetSikcaSorulanSoruVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<SikcaSorulanSoruDetailVm>> GetSikcaSorulanSoruByIdAsync(int id)
        {
            _logger.LogInformation("SSS getiriliyor. Id: {Id}", id);
            var response = await _sikcaSorulanSoruClient.GetSikcaSorulanSoruByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<SikcaSorulanSoruDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "SSS bulunamadı");
            }

            return ServiceResult<SikcaSorulanSoruDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<bool>> CreateSikcaSorulanSoruAsync(CreateSikcaSorulanSoruVm dto)
        {
            _logger.LogInformation("Yeni SSS oluşturuluyor. Soru: {Soru}", dto.Soru);
            var response = await _sikcaSorulanSoruClient.CreateSikcaSorulanSoruAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<bool>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "SSS oluşturulamadı");
            }

            _logger.LogInformation("SSS oluşturuldu.");
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> UpdateSikcaSorulanSoruAsync(SikcaSorulanSoruDetailVm dto)
        {
            _logger.LogInformation("SSS güncelleniyor. Id: {Id}", dto.Id);
            var response = await _sikcaSorulanSoruClient.UpdateSikcaSorulanSoruAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<bool>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"SSS güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("SSS güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> DeleteSikcaSorulanSoruAsync(int id)
        {
            _logger.LogWarning("SSS silme isteği alındı. Id: {Id}", id);
            var response = await _sikcaSorulanSoruClient.DeleteSikcaSorulanSoruAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<bool>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "SSS silinemedi");
            }

            _logger.LogInformation("SSS silindi. Id: {Id}", id);
            return ServiceResult<bool>.Success(true);
        }
    }
}
