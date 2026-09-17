using Microservice.Web.Services.Interfaces;
using Microservice.Web.ViewModels.Iletisim;
using Microsoft.AspNetCore.Mvc;

namespace Microservice.Web.Controllers
{
    /// <summary>
    /// Iletisim formu gonderim islemleri. Sayfanin kendisi TemplateController
    /// (StaticPage) tarafindan render edilir; bu controller yalnizca POST'u karsilar.
    /// Route ozelligi, Program.cs'deki catch-all ({*path}) route'unun onune gecmek
    /// icin MapControllers ile attribute routing olarak tanimlanir.
    /// </summary>
    [Route("Iletisim")]
    public class IletisimController : Controller
    {
        private readonly IIletisimService _iletisimService;
        private readonly ILogger<IletisimController> _logger;

        public IletisimController(
            IIletisimService iletisimService,
            ILogger<IletisimController> logger)
        {
            _iletisimService = iletisimService;
            _logger = logger;
        }

        [HttpPost("Gonder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Gonder([FromForm] IletisimMesajiVm form, [FromQuery] string? returnUrl)
        {
            // Acik yonlendirme (open redirect) engeli: yalnizca site ici goreli yol kabul edilir.
            var target = IsSafeLocalPath(returnUrl) ? returnUrl! : "/";

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Iletisim formu dogrulama basarisiz. SiteId: {SiteId}", form.SiteId);
                TempData["IletisimHata"] = "Lütfen form alanlarını kontrol edin.";
                return LocalRedirect(target);
            }

            var result = await _iletisimService.GonderAsync(form);

            if (result.IsFail)
            {
                _logger.LogError(
                    "Iletisim mesaji gonderilemedi. SiteId: {SiteId}, Hata: {Hata}",
                    form.SiteId, result.Fail?.Detail ?? result.Fail?.Title);

                TempData["IletisimHata"] = result.Fail?.Detail
                    ?? result.Fail?.Title
                    ?? "Mesaj gönderilemedi. Lütfen daha sonra tekrar deneyin.";
                return LocalRedirect(target);
            }

            TempData["IletisimBasari"] = "Mesajınız başarıyla iletildi. Teşekkür ederiz.";
            return LocalRedirect(target);
        }

        private static bool IsSafeLocalPath(string? url)
        {
            return !string.IsNullOrWhiteSpace(url)
                && url.StartsWith('/')
                && !url.StartsWith("//", StringComparison.Ordinal)
                && !url.StartsWith("\\", StringComparison.Ordinal);
        }
    }
}
