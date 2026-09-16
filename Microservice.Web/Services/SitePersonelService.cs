using Microservice.Web.Clients.SitePersonelClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.SitePersonel;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class SitePersonelService : ISitePersonelService
    {
        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        private readonly ISitePersonelClientServices _personelClient;
        private readonly IRedisCacheService _redisCacheService;
        private readonly ILogger<SitePersonelService> _logger;

        public SitePersonelService(
            ISitePersonelClientServices personelClient,
            IRedisCacheService redisCacheService,
            ILogger<SitePersonelService> logger)
        {
            _personelClient = personelClient ?? throw new ArgumentNullException(nameof(personelClient));
            _redisCacheService = redisCacheService ?? throw new ArgumentNullException(nameof(redisCacheService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetPersonelVm>>> GetPersonelListAsync(int siteId)
        {
            var cacheKey = $"personel:list:{siteId}";

            var cached = await _redisCacheService.GetListAsync<GetPersonelVm>(cacheKey);
            if (cached is not null)
            {
                return ServiceResult<List<GetPersonelVm>>.Success(cached);
            }

            _logger.LogInformation("Personel listesi çekiliyor. SiteId: {SiteId}", siteId);

            var response = await _personelClient.GetPersonelListAsync(siteId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode, problemDetails?.Title, problemDetails?.Detail);

                return ServiceResult<List<GetPersonelVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Personeller alınamadı");
            }

            var personeller = response.Content ?? new List<GetPersonelVm>();

            await _redisCacheService.SetListAsync(cacheKey, personeller, CacheDuration);

            return ServiceResult<List<GetPersonelVm>>.Success(personeller);
        }

        public async Task<ServiceResult<PersonelDetailVm>> GetPersonelByIdAsync(int id)
        {
            _logger.LogInformation("Personel getiriliyor. Id: {Id}", id);

            var response = await _personelClient.GetPersonelByIdAsync(id);

            if (!response.IsSuccessStatusCode || response.Content is null)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode, problemDetails?.Title, problemDetails?.Detail);

                return ServiceResult<PersonelDetailVm>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Personel bulunamadı");
            }

            return ServiceResult<PersonelDetailVm>.Success(response.Content);
        }
    }
}
