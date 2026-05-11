using Microservice.Admin.Clients.SikcaSorulanSoruKategoriClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.SikcaSorulanSoruKategori;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class SikcaSorulanSoruKategoriService : ISikcaSorulanSoruKategoriService
    {
        private readonly ISikcaSorulanSoruKategoriClientServices _kategoriClient;
        private readonly ILogger<SikcaSorulanSoruKategoriService> _logger;

        public SikcaSorulanSoruKategoriService(ISikcaSorulanSoruKategoriClientServices kategoriClient, ILogger<SikcaSorulanSoruKategoriService> logger)
        {
            _kategoriClient = kategoriClient ?? throw new ArgumentNullException(nameof(kategoriClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetSikcaSorulanSoruKategoriVm>>> GetSikcaSorulanSoruKategorilerAsync()
        {
            _logger.LogInformation("SSS kategori listesi çekiliyor.");
            var response = await _kategoriClient.GetSikcaSorulanSoruKategorilerAsync();

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetSikcaSorulanSoruKategoriVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "SSS kategorileri alınamadı");
            }

            return ServiceResult<List<GetSikcaSorulanSoruKategoriVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<SikcaSorulanSoruKategoriVm>> GetSikcaSorulanSoruKategoriByIdAsync(int id)
        {
            _logger.LogInformation("SSS kategori getiriliyor. Id: {Id}", id);
            var response = await _kategoriClient.GetSikcaSorulanSoruKategoriByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<SikcaSorulanSoruKategoriVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "SSS kategori bulunamadı");
            }

            return ServiceResult<SikcaSorulanSoruKategoriVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<bool>> CreateSikcaSorulanSoruKategoriAsync(SikcaSorulanSoruKategoriVm dto)
        {
            _logger.LogInformation("Yeni SSS kategori oluşturuluyor. Ad: {Ad}", dto.Ad);
            var response = await _kategoriClient.CreateSikcaSorulanSoruKategoriAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<bool>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "SSS kategori oluşturulamadı");
            }

            _logger.LogInformation("SSS kategori oluşturuldu.");
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> UpdateSikcaSorulanSoruKategoriAsync(SikcaSorulanSoruKategoriVm dto)
        {
            _logger.LogInformation("SSS kategori güncelleniyor. Id: {Id}", dto.Id);
            var response = await _kategoriClient.UpdateSikcaSorulanSoruKategoriAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<bool>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"SSS kategori güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("SSS kategori güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> DeleteSikcaSorulanSoruKategoriAsync(int id)
        {
            _logger.LogWarning("SSS kategori silme isteği alındı. Id: {Id}", id);
            var response = await _kategoriClient.DeleteSikcaSorulanSoruKategoriAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<bool>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "SSS kategori silinemedi");
            }

            _logger.LogInformation("SSS kategori silindi. Id: {Id}", id);
            return ServiceResult<bool>.Success(true);
        }
    }
}