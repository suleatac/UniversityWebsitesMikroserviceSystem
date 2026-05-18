using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.AuditLog;
using Refit;

namespace Microservice.Admin.Clients.AuditLogClients
{
    public interface IAuditLogClientServices
    {
        [Get("/api/v1/audit-logs")]
        Task<ApiResponse<List<AuditLogVm>>> GetAuditLogsAsync();

        [Get("/api/v1/audit-logs/{id}")]
        Task<ApiResponse<AuditLogDetailVm>> GetAuditLogByIdAsync(int id);

        [Post("/api/v1/audit-logs")]
        Task<ApiResponse<object>> CreateAuditLogAsync([Body] CreateAuditLogVm dto);

        [Delete("/api/v1/audit-logs/{id}")]
        Task<ApiResponse<object>> DeleteAuditLogAsync(int id);

        [Get("/api/v1/audit-logs/paginated")]
        Task<ApiResponse<PaginatedResult<AuditLogVm>>> GetAuditLogsPaginatedAsync(
          [AliasAs("page")] int page,
          [AliasAs("pageSize")] int pageSize,
          [AliasAs("search")] string? search,
          [AliasAs("orderBy")] string? orderBy,
          [AliasAs("orderDir")] string? orderDir);
    }
}
