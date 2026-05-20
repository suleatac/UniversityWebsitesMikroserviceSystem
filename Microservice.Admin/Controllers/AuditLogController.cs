using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.AuditLog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class AuditLogController : Controller
    {
        private readonly IAuditLogService _auditLogService;
        private readonly ILogger<AuditLogController> _logger;

        public AuditLogController(IAuditLogService auditLogService, ILogger<AuditLogController> logger)
        {
            _auditLogService = auditLogService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditLogsForPagination(
            int page = 1,
            int pageSize = 10,
            string search = "",
            int orderColumn = 0,
            string orderDir = "desc",
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            // Convert column index to name
            var columnName = orderColumn switch
            {
                1 => "Username",
                2 => "Action",
                3 => "EntityName",
                4 => "IpAddress",
                5 => "CreatedAt",
                _ => "Id"
            };

            var result = await _auditLogService.GetAuditLoglarPaginatedAsync(
                0, 0, page, pageSize, search, columnName, orderDir, startDate, endDate);

            if (!result.IsSuccess)
            {
                _logger.LogError("Paginated audit log listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                return BadRequest(new { error = result.Fail?.Detail });
            }

            return Ok(new
            {
                data = result.Data!.Data,
                recordsTotal = result.Data.TotalCount,
                recordsFiltered = result.Data.TotalCount
            });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            _logger.LogInformation("Audit log detay sayfası açılıyor. Id: {Id}", id);

            var result = await _auditLogService.GetAuditLogByIdAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogWarning("Audit log bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data!);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Audit log delete sayfası açılıyor. Id: {Id}", id);

            var result = await _auditLogService.GetAuditLogByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Audit log bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kayıt bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogWarning("Audit log silme isteği alındı. Id: {Id}", id);

            var result = await _auditLogService.DeleteAuditLogAsync(id);

            if (!result.IsSuccess)
            {
                _logger.LogError("Audit log silinemedi. Id: {Id}, Hata: {Error}", id, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? result.Fail?.Title ?? "Silme işlemi başarısız";
                return RedirectToAction(nameof(Index));
            }

            _logger.LogInformation("Audit log başarıyla silindi. Id: {Id}", id);
            TempData["Success"] = "Kayıt başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
