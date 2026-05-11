using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
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
        public IActionResult Create()
        {
            return View();
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserAddVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.CreateAccount(model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Fail?.Detail ?? "Kullanıcı eklenemedi");
                return View(model);
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

            var updateModel = new UserUpdateVm
            {
                Id = result.Data!.Id,
                FirstName = result.Data.FirstName,
                LastName = result.Data.LastName,
                Email = result.Data.Email,
                Enabled = result.Data.Enabled
            };

            return View(updateModel);
        }

        // UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserUpdateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var result = await _userService.UpdateAccount(model.Id, model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.Fail?.Detail ?? "Kullanıcı güncellenemedi");
                return View(model);
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
