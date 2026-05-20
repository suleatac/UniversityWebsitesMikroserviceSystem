using Microservice.Admin.Clients.AuditLogClients;
using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.AuditLog;
using System.Text.Json;

namespace Microservice.Admin.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogClientServices _auditLogClient;
        private readonly ILogger<AuditLogService> _logger;

        public AuditLogService
            (
            IAuditLogClientServices auditLogClient, 
            ILogger<AuditLogService> logger
            )
        {
            _auditLogClient = auditLogClient ?? throw new ArgumentNullException(nameof(auditLogClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<ServiceResult<AuditLogDetailVm>> GetAuditLogByIdAsync(int id)
        {
            _logger.LogInformation("Audit log getiriliyor. Id: {Id}", id);
            var response = await _auditLogClient.GetAuditLogByIdAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<AuditLogDetailVm>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Audit log bulunamadı");
            }

            return ServiceResult<AuditLogDetailVm>.Success(response.Content!);
        }

        public async Task<ServiceResult<object>> CreateAuditLogAsync(CreateAuditLogVm dto)
        {
            _logger.LogInformation("Yeni audit log oluşturuluyor. Başlık: {Title}", dto.UserId);
            var response = await _auditLogClient.CreateAuditLogAsync(dto);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Audit log oluşturulamadı");
            }

            _logger.LogInformation("Audit log oluşturuldu.");
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<object>> DeleteAuditLogAsync(int id)
        {
            _logger.LogWarning("Audit log silme isteği alındı. Id: {Id}", id);
            var response = await _auditLogClient.DeleteAuditLogAsync(id);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}, Detail: {Detail}", response.StatusCode, problemDetails?.Title, problemDetails?.Detail);
                return ServiceResult<object>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Audit log silinemedi");
            }

            _logger.LogInformation("Audit log silindi. Id: {Id}", id);
            return ServiceResult<object>.Success(true);
        }

        public async Task<ServiceResult<PaginatedResult<AuditLogVm>>> GetAuditLoglarPaginatedAsync(int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir, DateTime? startDate = null, DateTime? endDate = null)
        {
            _logger.LogInformation("Paginated audit log listesi çekiliyor. Page: {Page}, PageSize: {PageSize}", page, pageSize);
            var response = await _auditLogClient.GetAuditLogsPaginatedAsync(page, pageSize, search, orderBy, orderDir, startDate, endDate);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}", response.StatusCode, problemDetails?.Title);
                return ServiceResult<PaginatedResult<AuditLogVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Paginated audit log listesi alınamadı");
            }

            return ServiceResult<PaginatedResult<AuditLogVm>>.Success(response.Content!);
        }

        public async Task<ServiceResult<List<AuditLogDailyStatVm>>> GetAuditLogDailyStatsAsync(DateTime startDate, DateTime endDate)
        {
            _logger.LogInformation("Günlük audit log istatistikleri çekiliyor. {StartDate} - {EndDate}", startDate.ToString("yyyy-MM-dd"), endDate.ToString("yyyy-MM-dd"));
            var response = await _auditLogClient.GetAuditLogDailyStatsAsync(startDate, endDate);

            if (!response.IsSuccessStatusCode)
            {
                var problemDetails = response.Error != null
                    ? JsonSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(response.Error.Content!) : null;
                _logger.LogError("API Error -> StatusCode: {StatusCode}, Title: {Title}", response.StatusCode, problemDetails?.Title);
                return ServiceResult<List<AuditLogDailyStatVm>>.Error(problemDetails?.Detail ?? problemDetails?.Title ?? "Günlük istatistikler alınamadı");
            }

            return ServiceResult<List<AuditLogDailyStatVm>>.Success(response.Content!);
        }
    }
}
