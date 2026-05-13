using Microservice.Admin.Services.Interfaces;
using Microservice.Admin.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Admin.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly IProfileService _profileService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(
            IProfileService profileService,
            ICurrentUserService currentUserService,
            ILogger<ProfileController> logger)
        {
            _profileService = profileService;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Profil sayfası açıldı.");

            var keycloakUserId = _currentUserService.KeycloakUserId;
            var result = await _profileService.GetCurrentUserProfileAsync(keycloakUserId);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Profil bulunamadı. UserId: {UserId}", keycloakUserId);
                TempData["Error"] = result.Fail?.Detail ?? "Profil bilgileri alınamadı.";
                return View(new ProfileVm());
            }

            return View(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            _logger.LogInformation("Profil düzenleme sayfası açıldı.");

            var keycloakUserId = _currentUserService.KeycloakUserId;
            var result = await _profileService.GetCurrentUserProfileAsync(keycloakUserId);

            if (!result.IsSuccess || result.Data == null)
            {
                _logger.LogWarning("Profil bulunamadı. UserId: {UserId}", keycloakUserId);
                TempData["Error"] = "Profil bilgileri alınamadı.";
                return RedirectToAction(nameof(Index));
            }

            return View(result.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileVm model)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Profil güncelleme - ModelState geçersiz.");
                return View(model);
            }

            var keycloakUserId = _currentUserService.KeycloakUserId;
            var result = await _profileService.UpdateUserProfileAsync(keycloakUserId, model);

            if (!result.IsSuccess)
            {
                _logger.LogError("Profil güncellenemedi. UserId: {UserId}, Hata: {Error}", keycloakUserId, result.Fail?.Detail);
                ModelState.AddModelError("", result.Fail?.Detail ?? result.Fail?.Title ?? "Profil güncellenemedi.");
                return View(model);
            }

            _logger.LogInformation("Profil güncellendi. UserId: {UserId}", keycloakUserId);
            TempData["Success"] = "Profil başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }
    }
}