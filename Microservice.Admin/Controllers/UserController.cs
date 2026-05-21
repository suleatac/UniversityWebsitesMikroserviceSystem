using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.TumPersonel;
using Microservice.Admin.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ITumPersonelService _tumPersonelService;
        private readonly IKeycloakRoleService _keycloakRoleService;
        private readonly ILogger<UserController> _logger;

        public UserController(
            IUserService userService,
            ITumPersonelService tumPersonelService,
            IKeycloakRoleService keycloakRoleService,
            ILogger<UserController> logger)
        {
            _userService = userService;
            _tumPersonelService = tumPersonelService;
            _keycloakRoleService = keycloakRoleService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersForPagination(
            int page = 1,
            int pageSize = 10,
            string search = "",
            int orderColumn = 0,
            string orderDir = "desc")
        {
            // Sütun indeksini isimlere çevir
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

        // CREATE - GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var personelResult = await _tumPersonelService.GetTumPersonelsAsync();
            var rolesResult = await _keycloakRoleService.GetRealmRolesAsync();

            var model = new UserCreateIndexVm
            {
                UserAdd = UserAddVm.Empty,
                TumPersoneller = personelResult.IsSuccess ? personelResult.Data ?? new List<GetPersonelVm>() : new List<GetPersonelVm>(),
                AvailableRoles = rolesResult.IsSuccess ? rolesResult.Data ?? new List<KeycloakRoleVm>() : new List<KeycloakRoleVm>()
            };
            return View(model);
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserCreateIndexVm model)
        {
            if (!ModelState.IsValid)
            {
                var personelResult = await _tumPersonelService.GetTumPersonelsAsync();
                var rolesResult = await _keycloakRoleService.GetRealmRolesAsync();
                model.TumPersoneller = personelResult.IsSuccess ? personelResult.Data ?? new List<GetPersonelVm>() : new List<GetPersonelVm>();
                model.AvailableRoles = rolesResult.IsSuccess ? rolesResult.Data ?? new List<KeycloakRoleVm>() : new List<KeycloakRoleVm>();
                return View(model);
            }

            var result = await _userService.CreateAccount(model.UserAdd);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Fail?.Detail ?? "Kullanıcı eklenemedi");
                var personelResult = await _tumPersonelService.GetTumPersonelsAsync();
                var rolesResult = await _keycloakRoleService.GetRealmRolesAsync();
                model.TumPersoneller = personelResult.IsSuccess ? personelResult.Data ?? new List<GetPersonelVm>() : new List<GetPersonelVm>();
                model.AvailableRoles = rolesResult.IsSuccess ? rolesResult.Data ?? new List<KeycloakRoleVm>() : new List<KeycloakRoleVm>();
                return View(model);
            }

            // Kullanıcı oluşturulduktan sonra seçilen rolleri ata
            var userId = result.Data;
            if (!string.IsNullOrEmpty(userId) && model.UserAdd.SelectedRoles != null && model.UserAdd.SelectedRoles.Any())
            {
                var roleResult = await _keycloakRoleService.AssignRolesToUserAsync(userId, model.UserAdd.SelectedRoles.ToList());
                if (!roleResult.IsSuccess)
                {
                    _logger.LogWarning("Kullanıcı oluşturuldu ancak roller atanamadı. UserId: {UserId}, Hata: {Error}", userId, roleResult.Fail?.Detail);
                    TempData["Warning"] = "Kullanıcı oluşturuldu ancak bazı roller atanamadı: " + (roleResult.Fail?.Detail ?? "Bilinmeyen hata");
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // UPDATE - GET
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var result = await _userService.GetUserByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound();

            // Kullanıcının mevcut rollerini al
            var assignedRolesResult = await _keycloakRoleService.GetUserRolesAsync(id);
            var allRolesResult = await _keycloakRoleService.GetRealmRolesAsync();

            var assignedRoles = assignedRolesResult.IsSuccess ? assignedRolesResult.Data ?? new List<KeycloakRoleVm>() : new List<KeycloakRoleVm>();
            var allRoles = allRolesResult.IsSuccess ? allRolesResult.Data ?? new List<KeycloakRoleVm>() : new List<KeycloakRoleVm>();

            var updateModel = new UserUpdateVm
            {
                Id = result.Data!.Id,
                FirstName = result.Data.FirstName,
                LastName = result.Data.LastName,
                Email = result.Data.Email,
                Enabled = result.Data.Enabled,
                SelectedRoles = assignedRoles.Select(r => r.Name).ToList(),
                AvailableRoles = allRoles
                    .Where(r => !assignedRoles.Any(ar => ar.Id == r.Id))
                    .ToList(),
                AssignedRoles = assignedRoles
            };

            return View(updateModel);
        }

        // UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserUpdateVm model)
        {
            if (!ModelState.IsValid)
            {
                // Reload roles for the view
                var assignedRolesResult = await _keycloakRoleService.GetUserRolesAsync(model.Id);
                var allRolesResult = await _keycloakRoleService.GetRealmRolesAsync();
                var assignedRoles = assignedRolesResult.IsSuccess ? assignedRolesResult.Data ?? new List<KeycloakRoleVm>() : new List<KeycloakRoleVm>();
                var allRoles = allRolesResult.IsSuccess ? allRolesResult.Data ?? new List<KeycloakRoleVm>() : new List<KeycloakRoleVm>();
                model.AvailableRoles = allRoles.Where(r => !assignedRoles.Any(ar => ar.Id == r.Id)).ToList();
                model.AssignedRoles = assignedRoles;
                return View(model);
            }

            // Kullanıcı bilgilerini güncelle
            var result = await _userService.UpdateAccount(model.Id, model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Fail?.Detail ?? "Kullanıcı güncellenemedi");
                var assignedRolesResult = await _keycloakRoleService.GetUserRolesAsync(model.Id);
                var allRolesResult = await _keycloakRoleService.GetRealmRolesAsync();
                var assignedRoles = assignedRolesResult.IsSuccess ? assignedRolesResult.Data ?? new List<KeycloakRoleVm>() : new List<KeycloakRoleVm>();
                var allRoles = allRolesResult.IsSuccess ? allRolesResult.Data ?? new List<KeycloakRoleVm>() : new List<KeycloakRoleVm>();
                model.AvailableRoles = allRoles.Where(r => !assignedRoles.Any(ar => ar.Id == r.Id)).ToList();
                model.AssignedRoles = assignedRoles;
                return View(model);
            }

            // Roller güncelle: eski rolleri al, farkı hesapla ve ekle/kaldır
            var currentRolesResult = await _keycloakRoleService.GetUserRolesAsync(model.Id);
            var currentRoleNames = currentRolesResult.IsSuccess
                ? currentRolesResult.Data?.Select(r => r.Name).ToList() ?? new List<string>()
                : new List<string>();

            var selectedRoles = model.SelectedRoles ?? new List<string>();

            var rolesToAdd = selectedRoles.Except(currentRoleNames, StringComparer.OrdinalIgnoreCase).ToList();
            var rolesToRemove = currentRoleNames.Except(selectedRoles, StringComparer.OrdinalIgnoreCase).ToList();

            if (rolesToAdd.Any())
            {
                var addResult = await _keycloakRoleService.AssignRolesToUserAsync(model.Id, rolesToAdd);
                if (!addResult.IsSuccess)
                {
                    _logger.LogWarning("Bazı roller atanamadı. UserId: {UserId}, Hata: {Error}", model.Id, addResult.Fail?.Detail);
                }
            }

            if (rolesToRemove.Any())
            {
                var removeResult = await _keycloakRoleService.RemoveRolesFromUserAsync(model.Id, rolesToRemove);
                if (!removeResult.IsSuccess)
                {
                    _logger.LogWarning("Bazı roller kaldırılamadı. UserId: {UserId}, Hata: {Error}", model.Id, removeResult.Fail?.Detail);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // DELETE - GET
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            _logger.LogInformation("User delete sayfası açıldı. Id: {Id}", id);

            var result = await _userService.GetUserByIdAsync(id);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Kullanıcı bulunamadı. Id: {Id}", id);
                TempData["Error"] = "Silinecek kullanıcı bulunamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        // DELETE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var result = await _userService.DeleteAccount(id);

            if (!result.IsSuccess)
            {
                TempData["Error"] = result.Fail?.Detail ?? "Kullanıcı silinemedi";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
