using Microservice.Admin.Clients.HaberClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.Haber;
using Microservice.Admin.ViewModels.Site;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class HaberService : IHaberService
    {
        private readonly IHaberClientService _haberClient;
        private readonly ILogger<HaberService> _logger;

        public HaberService(IHaberClientService haberClient, ILogger<HaberService> logger)
        {
            _haberClient = haberClient ?? throw new ArgumentNullException(nameof(haberClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // LIST
        public async Task<ServiceResult<List<GetHaberVm>>> GetHabersAsync(int siteId, int dilId)
        {
            _logger.LogInformation("Haber listesi çekiliyor. SiteId: {SiteId}, DilId: {DilId}", siteId, dilId);

            var response = await _haberClient.GetHabersAsync(siteId, dilId);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<List<GetHaberVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Haberler alınamadı"
                );
            }

            _logger.LogInformation("Haber listesi alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<GetHaberVm>>.Success(response.Content!);
        }



        //Haber paginated list
        public async Task<ServiceResult<PaginatedResult<GetHaberVm>>> GetHabersPaginatedAsync
            (
               int siteId, 
               int dilId,
               int page, 
               int pageSize, 
               string? search, 
               string? orderBy, 
               string? orderDir
            )
        {
            _logger.LogInformation("API'den paginated haber listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);

            var response = await _haberClient.GetHabersPaginatedAsync(siteId, dilId, page, pageSize, search, orderBy, orderDir);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}",
                    response.StatusCode, problemDetails?.Title);

                return ServiceResult<PaginatedResult<GetHaberVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated haber listesi alınamadı");
            }

            _logger.LogInformation("Paginated haber listesi başarıyla alındı. TotalCount: {TotalCount}",
                response.Content?.TotalCount);

            return ServiceResult<PaginatedResult<GetHaberVm>>.Success(response.Content!);
        }


        // GET BY ID
        public async Task<ServiceResult<HaberDetailVm>> GetHaberByIdAsync(int id)
        {
            _logger.LogInformation("Haber getiriliyor. Id: {Id}", id);

            var response = await _haberClient.GetHaberByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<HaberDetailVm>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Haber bulunamadı"
                );
            }

            return ServiceResult<HaberDetailVm>.Success(response.Content!);
        }

        // CREATE
        public async Task<ServiceResult<object>> CreateHaberAsync(CreateHaberVm dto)
        {
            _logger.LogInformation("Yeni haber oluşturuluyor. Başlık: {Title}", dto.Baslik);


            var testVerisi = new CreateHaberVm {
                SiteId = 1,                          // ✅ 0'dan büyük olmalı
                DilId = 1,                           // ✅ 0'dan büyük olmalı
                HedefId = 1,                      // ✅ Opsiyonel

                Baslik = "Üniversitemizde Yeni Kütüphane Binası Açıldı",           // ✅ Boş olamaz, max 200 karakter
                KisaAciklama = "Modern mimarisi ve geniş koleksiyonuyla yeni kütüphane binamız hizmete girdi.", // ✅ Boş olamaz, max 500 karakter
                IcerikMetni = "<p>Üniversitemizin yeni kütüphane binası hizmete girdi.</p>", // ✅ Boş olamaz

                Link = "https://www.ornek-universite.edu.tr/kutuphane",            // ✅ Opsiyonel, geçerli URL olmalı
                ResimUrl = "https://www.ornek-universite.edu.tr/images/kutuphane.jpg", // ✅ Opsiyonel, geçerli URL olmalı

                YayimTarihi = DateTime.Now,          // ✅ Boş olamaz
                BaslamaTarihi = DateTime.Today,      // ✅ Opsiyonel
                BitisTarihi = null,                  // ✅ Opsiyonel

                SeoUrl = "universitemizde-yeni-kutuphane-binasi-acildi",  // ✅ Opsiyonel
                SeoTitle = "Yeni Kütüphane Binası",                       // ✅ Opsiyonel
                SeoDescription = "Yeni kütüphane binamız hizmete girdi."  // ✅ Opsiyonel
            };



            var response = await _haberClient.CreateHaberAsync(testVerisi);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<object>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Haber oluşturulamadı"
                );
            }

            _logger.LogInformation("Haber oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        // UPDATE
        public async Task<ServiceResult<object>> UpdateHaberAsync(HaberDetailVm dto)
        {
            _logger.LogInformation("Haber güncelleniyor. Id: {Id}", dto.Id);

            var response = await _haberClient.UpdateHaberAsync(dto.Id, dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<object>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? $"Haber güncellenemedi. Id: {dto.Id}"
                );
            }

            _logger.LogInformation("Haber güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<object>.Success(true);
        }

        // DELETE
        public async Task<ServiceResult<object>> DeleteHaberAsync(int id)
        {
            _logger.LogWarning("Haber siliniyor. Id: {Id}", id);

            var response = await _haberClient.DeleteHaberAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError(
                    "API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}",
                    response.StatusCode,
                    problemDetails?.Title,
                    problemDetails?.Detail
                );

                return ServiceResult<object>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? $"Haber silinemedi. Id: {id}"
                );
            }

            _logger.LogInformation("Haber silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }
    }
}
