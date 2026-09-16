using Microservice.Admin.Clients.PersonelClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels.Personel;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class PersonelService : IPersonelService
    {

        private readonly IPersonelClientServices _personelClient;
        private readonly ILogger<PersonelService> _logger;

        public PersonelService(
            IPersonelClientServices personelClient,
            ILogger<PersonelService> logger

            )
        {
            _personelClient = personelClient ?? throw new ArgumentNullException(nameof(personelClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<ServiceResult<List<GetPersonelVm>>> GetSitePersonellerAsync()
        {
            _logger.LogInformation("Personel listesi çekiliyor.");
            var response = await _personelClient.GetPersonellerAsync();

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetPersonelVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Personeller alınamadı");
            }

            return ServiceResult<List<GetPersonelVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<PersonelDetailVm>> GetPersonelByIdAsync(int id)
        {
            _logger.LogInformation("Personel getiriliyor. Id: {Id}", id);
            var response = await _personelClient.GetPersonelByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<PersonelDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Personel bulunamadı");
            }

            return ServiceResult<PersonelDetailVm>.Success(response.Content!);
        }
    }
}
