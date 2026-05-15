using Microservice.Admin.Clients.YonetimDuyuruClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.YonetimDuyuru;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class YonetimDuyuruService: IYonetimDuyuruService
    {
        private readonly IYonetimDuyuruClientServices _yonetimDuyuruRefitService;
        private readonly ILogger<YonetimDuyuruService> _logger;

        public YonetimDuyuruService(IYonetimDuyuruClientServices yonetimDuyuruRefitService, ILogger<YonetimDuyuruService> logger)
        {
            _yonetimDuyuruRefitService = yonetimDuyuruRefitService ?? throw new ArgumentNullException(nameof(yonetimDuyuruRefitService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        // LIST
        public async Task<ServiceResult<List<YonetimDuyuruVm>>> GetYonetimDuyurusAsync()
        {
            _logger.LogInformation("API'den yonetim duyurusu listesi çekiliyor.");

            var response = await _yonetimDuyuruRefitService.GetYonetimDuyurusAsync();

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


                return ServiceResult<List<YonetimDuyuruVm>>.Error(
                problemDetails?.Detail ?? problemDetails?.Title ?? "Yonetim duyuruları alınamadı"
            );
            }

            _logger.LogInformation("Yonetim duyuru listesi başarıyla alındı. Count: {Count}", response.Content?.Count);
            return ServiceResult<List<YonetimDuyuruVm>>.Success(response.Content!);
        }

        //YonetimDuyuru paginated list
        public async Task<ServiceResult<PaginatedResult<YonetimDuyuruVm>>> GetYonetimDuyuruPaginatedAsync(
     int page, int pageSize, string? search, string? orderBy, string? orderDir)
        {
            _logger.LogInformation("API'den paginated yonetim duyuru listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);

            var response = await _yonetimDuyuruRefitService.GetYonetimDuyuruPaginatedAsync(page, pageSize, search, orderBy, orderDir);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!)
                    : null;

                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}",
                    response.StatusCode, problemDetails?.Title);

                return ServiceResult<PaginatedResult<YonetimDuyuruVm>>.Error(
                    problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated yonetim duyuru listesi alınamadı");
            }

            _logger.LogInformation("Paginated yonetim duyuru listesi başarıyla alındı. TotalCount: {TotalCount}",
                response.Content?.TotalCount);

            return ServiceResult<PaginatedResult<YonetimDuyuruVm>>.Success(response.Content!);
        }
        // GET BY ID
        public async Task<ServiceResult<YonetimDuyuruVm>> GetYonetimDuyuruByIdAsync(int id)
        {
            _logger.LogInformation("Yonetim duyuru getiriliyor. Id: {Id}", id);

            var response = await _yonetimDuyuruRefitService.GetYonetimDuyuruByIdAsync(id);

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

                return ServiceResult<YonetimDuyuruVm>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Yonetim duyuruları alınamadı");

            }

            return ServiceResult<YonetimDuyuruVm>.Success(response.Content!);
        }

        // CREATE
        public async Task<ServiceResult<object>> CreateYonetimDuyuruAsync(YonetimDuyuruVm dto)
        {
            _logger.LogInformation("Yeni yonetim duyurusu oluşturuluyor. Name: {Name}", dto.Baslik);

            var response = await _yonetimDuyuruRefitService.CreateYonetimDuyuruAsync(dto);

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

                return ServiceResult<object>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Yonetim duyurusu oluşturulamadı");

            }

            _logger.LogInformation("Yonetim duyurusu başarıyla oluşturuldu. Id: {Id}", response.Content);
            return ServiceResult<object>.Success(true);
        }

        // UPDATE
        public async Task<ServiceResult<bool>> UpdateYonetimDuyuruAsync(YonetimDuyuruVm dto)
        {
            _logger.LogInformation("Yonetim duyurusu güncelleniyor. Id: {Id}", dto.Id);

            var response = await _yonetimDuyuruRefitService.UpdateYonetimDuyuruAsync(dto.Id, dto);

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

                return ServiceResult<bool>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Site güncellenemedi. Id: {dto.Id}");

            }

            _logger.LogInformation("Yonetim duyurusu güncellendi. Id: {Id}", dto.Id);
            return ServiceResult<bool>.Success(true);
        }

        // DELETE
        public async Task<ServiceResult<bool>> DeleteYonetimDuyuruAsync(int id)
        {
            _logger.LogWarning("Yonetim duyurusu siliniyor. Id: {Id}", id);

            var response = await _yonetimDuyuruRefitService.DeleteYonetimDuyuruAsync(id);

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

                return ServiceResult<bool>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Site silinemedi. Id: {id}");

            }

            _logger.LogInformation("Yonetim duyurusu silindi. Id: {Id}", id);
            return ServiceResult<bool>.Success(true);
        }


        // GET DETAIL
        public async Task<ServiceResult<YonetimDuyuruDetailVm>> GetYonetimDuyuruDetailAsync(int id)
        {
            _logger.LogInformation("Yonetim duyuru detayi getiriliyor. Id: {Id}", id);

            var response = await _yonetimDuyuruRefitService.GetYonetimDuyuruDetailAsync(id);

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

                return ServiceResult<YonetimDuyuruDetailVm>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Yonetim duyuru detayi alinamadi");

            }

            return ServiceResult<YonetimDuyuruDetailVm>.Success(response.Content!);
        }

        // MARK AS READ
        public async Task<ServiceResult<bool>> MarkYonetimDuyuruAsReadAsync(int id)
        {
            _logger.LogInformation("Yonetim duyuru okundu olarak isaretleniyor. Id: {Id}", id);

            var response = await _yonetimDuyuruRefitService.MarkYonetimDuyuruAsReadAsync(id);

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

                return ServiceResult<bool>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? $"Duyuru okundu olarak isaretlenemedi. Id: {id}");

            }

            _logger.LogInformation("Yonetim duyuru okundu olarak isaretlendi. Id: {Id}", id);
            return ServiceResult<bool>.Success(true);
        }

        // GET UNREAD COUNT
        public async Task<ServiceResult<int>> GetUnreadYonetimDuyuruCountAsync()
        {
            _logger.LogInformation("Okunmamis yonetim duyuru sayisi getiriliyor.");

            var response = await _yonetimDuyuruRefitService.GetUnreadYonetimDuyuruCountAsync();

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

                return ServiceResult<int>
                    .Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Okunmamis duyuru sayisi alinamadi");

            }

            return ServiceResult<int>.Success(response.Content);
        }

    }
}
