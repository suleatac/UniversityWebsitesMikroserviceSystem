using Microservice.Web.Clients.GaleriResimClients;
using Microservice.Web.Services.Interfaces;
using Microservice.Web.Services.ServiceResults;
using Microservice.Web.ViewModels.GaleriResim;
using Microservice.Web.ViewModels.Paged;
using System.Text.Json;

namespace Microservice.Web.Services
{
    public class GaleriResimService : IGaleriResimService
    {
        private readonly IGaleriResimClientServices _galeriResimClient;
        private readonly ILogger<GaleriResimService> _logger;

        public GaleriResimService(IGaleriResimClientServices galeriResimClient, ILogger<GaleriResimService> logger)
        {
            _galeriResimClient = galeriResimClient ?? throw new ArgumentNullException(nameof(galeriResimClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<List<GetGaleriResimVm>>> GetGaleriResimlerAsync(int siteId, int dilId)
        {
            _logger.LogInformation("Galeri resmi listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);
            var response = await _galeriResimClient.GetGaleriResimlerAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<List<GetGaleriResimVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Galeri resimleri alınamadı");
            }

            return ServiceResult<List<GetGaleriResimVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<GaleriResimDetailVm>> GetGaleriResimByIdAsync(int id)
        {
            _logger.LogInformation("Galeri resmi getiriliyor. Id: {Id}", id);
            var response = await _galeriResimClient.GetGaleriResimByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<GaleriResimDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Galeri resmi bulunamadı");
            }

            return ServiceResult<GaleriResimDetailVm>.Success(response.Content!);
        }

        // PAGINATED + SEARCH + KATEGORI
        public async Task<ServiceResult<PagedResultVm<GetGaleriResimVm>>> GetPaginatedAsync(
            int siteId,
            int dilId,
            string? search,
            string? kategori,
            int page,
            int pageSize)
        {
            _logger.LogInformation(
                "Sayfali galeri resmi listesi cekiliyor. SiteId: {SiteId}, DilId: {DilId}, Search: {Search}, Kategori: {Kategori}, Page: {Page}",
                siteId, dilId, search, kategori, page);

            // Liste siralamasi repository ile ayni: Sira asc, sonra YayimTarihi desc (handler'daki varsayilan).
            var response = await _galeriResimClient.GetPaginatedAsync(siteId, dilId, page, pageSize, search, kategori, "sira", "asc");

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode, problemDetails?.Title, problemDetails?.Detail);

                return ServiceResult<PagedResultVm<GetGaleriResimVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Galeri resimleri alınamadı");
            }

            return ServiceResult<PagedResultVm<GetGaleriResimVm>>.Success(
                response.Content ?? new PagedResultVm<GetGaleriResimVm>());
        }
    }
}
