using Microservice.Admin.Services.ServiceResults;
using Microservice.Admin.ViewModels;
using Microservice.Admin.ViewModels.AuditLog;

namespace Microservice.Admin.Services.Interfaces
{
    public interface IAuditLogService
    {
        Task<ServiceResult<AuditLogDetailVm>> GetAuditLogByIdAsync(int id);
        Task<ServiceResult<object>> CreateAuditLogAsync(CreateAuditLogVm dto);
        Task<ServiceResult<object>> DeleteAuditLogAsync(int id);
        Task<ServiceResult<PaginatedResult<AuditLogVm>>> GetAuditLoglarPaginatedAsync(int siteId, int dilId, int page, int pageSize, string? search, string? orderBy, string? orderDir);
    }
}
