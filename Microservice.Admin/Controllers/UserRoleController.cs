using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.User;
using Microservice.Admin.ViewModels.UserRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class UserRoleController : Controller
    {
        private readonly IUserService _userService;
        private readonly IKeycloakRoleService _keycloakRoleService;
        private readonly ILogger<UserRoleController> _logger;

        public UserRoleController(
            IUserService userService,
            IKeycloakRoleService keycloakRoleService,
            ILogger<UserRoleController> logger)
        {
            _userService = userService;
            _keycloakRoleService = keycloakRoleService;
            _logger = logger;
        }

        /// <summary>
        /// Kullanıcı listesini gösterir - her satırda rol yönetimi butonu vardır
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// DataTables için paginated kullanıcı listesi döner
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUsersForPagination(
            int page = 1,
            int pageSize = 10,
            string search = "",
            int orderColumn = 0,
            string orderDir = "desc")
        {
            var columnName = orderColumn switch
            {
                1 => "UserName",
                2 => "FirstName",
                3 => "LastName",
                4 => "Email",
                _ => "UserName"
            };

            var result = await _userService.GetUsersPaginatedAsync(page, pageSize, search, columnName, orderDir);

            if (!result.IsSuccess)
            {
                _logger.LogError("Paginated user listesi alınamadı. Hata: {Error}", result.Fail?.Detail);
                return BadRequest(new { error = result.Fail?.Detail });
            }

            return Ok(new
            {
                data = result.Data!.Data,
                recordsTotal = result.Data.TotalCount,
                recordsFiltered = result.Data.TotalCount
            });
        }

        /// <summary>
        /// Belirli bir kullanıcının rol yönetimi sayfası
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ManageRoles(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                TempData["Error"] = "Kullanıcı ID'si gereklidir.";
                return RedirectToAction(nameof(Index));
            }

            var userResult = await _userService.GetUserByIdAsync(id);
            if (!userResult.IsSuccess || userResult.Data == null)
            {
                TempData["Error"] = "Kullanıcı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            var assignedRolesResult = await _keycloakRoleService.GetUserRolesAsync(id);
            var allRolesResult = await _keycloakRoleService.GetRealmRolesAsync();

            var assignedRoles = assignedRolesResult.IsSuccess ? assignedRolesResult.Data ?? new List<KeycloakRoleVm>() : new List<KeycloakRoleVm>();
            var allRoles = allRolesResult.IsSuccess ? allRolesResult.Data ?? new List<KeycloakRoleVm>() : new List<KeycloakRoleVm>();

            // Sadece atanmamış rolleri göster
            var availableRoles = allRoles
                .Where(r => !assignedRoles.Any(ar => ar.Id == r.Id))
                .ToList();

            var viewModel = new UserRoleManageVm
            {
                User = userResult.Data,
                AssignedRoles = assignedRoles,
                AvailableRoles = availableRoles
            };

            return View(viewModel);
        }

        /// <summary>
        /// Kullanıcıya rol atar
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                TempData["Error"] = "Kullanıcı ID ve rol adı gereklidir.";
                return RedirectToAction(nameof(ManageRoles), new { id = userId });
            }

            var result = await _keycloakRoleService.AssignRoleToUserAsync(userId, roleName);

            if (!result.IsSuccess)
            {
                _logger.LogError("Rol atama başarısız. UserId: {UserId}, Role: {RoleName}, Hata: {Error}",
                    userId, roleName, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? "Rol atanamadı.";
            }
            else
            {
                _logger.LogInformation("Rol atandı. UserId: {UserId}, Role: {RoleName}", userId, roleName);
                TempData["Success"] = $"'{roleName}' rolü başarıyla atandı.";
            }

            return RedirectToAction(nameof(ManageRoles), new { id = userId });
        }

        /// <summary>
        /// Kullanıcıdan rol kaldırır
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveRole(string userId, string roleName)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
            {
                TempData["Error"] = "Kullanıcı ID ve rol adı gereklidir.";
                return RedirectToAction(nameof(ManageRoles), new { id = userId });
            }

            var result = await _keycloakRoleService.RemoveRoleFromUserAsync(userId, roleName);

            if (!result.IsSuccess)
            {
                _logger.LogError("Rol kaldırma başarısız. UserId: {UserId}, Role: {RoleName}, Hata: {Error}",
                    userId, roleName, result.Fail?.Detail);
                TempData["Error"] = result.Fail?.Detail ?? "Rol kaldırılamadı.";
            }
            else
            {
                _logger.LogInformation("Rol kaldırıldı. UserId: {UserId}, Role: {RoleName}", userId, roleName);
                TempData["Success"] = $"'{roleName}' rolü başarıyla kaldırıldı.";
            }

            return RedirectToAction(nameof(ManageRoles), new { id = userId });
        }

        /// <summary>
        /// AJAX ile kullanıcının Admin rolünde olup olmadığını kontrol eder
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> IsUserAdmin(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return Json(new { isAdmin = false });

            var isAdmin = await _keycloakRoleService.IsUserInRoleAsync(userId, "Admin");
            return Json(new { isAdmin });
        }
    }
}