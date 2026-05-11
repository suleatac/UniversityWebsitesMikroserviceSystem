using Microservice.Admin.Clients.PersonelTipClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.PersonelTip;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class PersonelTipService : IPersonelTipService
    {
        private readonly IPersonelTipClientServices _personelTipClient;
        private readonly ILogger<PersonelTipService> _logger;

        public PersonelTipService(IPersonelTipClientServices personelTipClient, ILogger<PersonelTipService> logger)
        {
            _personelTipClient = personelTipClient ?? throw new ArgumentNullException(nameof(personelTipClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetPersonelTipVm>>> GetPersonelTiplerAsync()
        {
            _logger.LogInformation("API'den personel tip listesi çekiliyor.");

            var response = await _personelTipClient.GetPersonelTiplerAsync();

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode, problemDetails?.Title, problemDetails?.Detail);

                return ServiceResult<List<GetPersonelTipVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Personel tipler alınamadı");
            }

            _logger.LogInformation("Personel tip listesi başarıyla alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<GetPersonelTipVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<PersonelTipVm>> GetPersonelTipByIdAsync(int id)
        {
            _logger.LogInformation("Personel tip getiriliyor. Id: {Id}", id);

            var response = await _personelTipClient.GetPersonelTipByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode, problemDetails?.Title, problemDetails?.Detail);

                return ServiceResult<PersonelTipVm>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Personel tip alınamadı");
            }

            return ServiceResult<PersonelTipVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<bool>> CreatePersonelTipAsync(PersonelTipVm dto)
        {
            _logger.LogInformation("Yeni personel tip oluşturuluyor. Ad: {Ad}", dto.Ad);

            var response = await _personelTipClient.CreatePersonelTipAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode, problemDetails?.Title, problemDetails?.Detail);

                return ServiceResult<bool>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Personel tip oluşturulamadı");
            }

            _logger.LogInformation("Personel tip oluşturuldu.");
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> UpdatePersonelTipAsync(PersonelTipVm dto)
        {
            _logger.LogInformation("Personel tip güncelleniyor. Id: {Id}", dto.Id);

            var response = await _personelTipClient.UpdatePersonelTipAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode, problemDetails?.Title, problemDetails?.Detail);

                return ServiceResult<bool>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? $"Personel tip güncellenemedi. Id: {dto.Id}");
            }

            _logger.LogInformation("Personel tip güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<bool>.Success(true);
        }

        public async Task<ServiceResult<bool>> DeletePersonelTipAsync(int id)
        {
            _logger.LogWarning("Personel tip silme isteği alındı. Id: {Id}", id);

            var response = await _personelTipClient.DeletePersonelTipAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode, problemDetails?.Title, problemDetails?.Detail);

                return ServiceResult<bool>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Personel tip silinemedi");
            }

            _logger.LogInformation("Personel tip silindi. Id: {Id}", id);
            return ServiceResult<bool>.Success(true);
        }
    }
}