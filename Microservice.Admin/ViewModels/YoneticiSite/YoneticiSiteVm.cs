using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.YoneticiSite
{
    // Mikroservice.Site.Application.Features.YoneticiSiteFeatures.CreateYoneticiSite.CreateYoneticiSiteCommandValidation
    // kurallariyla paralel tutulmustur (istemci tarafi on dogrulama).
    public class YoneticiSiteVm
    {

        [Required(ErrorMessage = "PersonelId zorunludur")]
        public string KeycloakUserId { get; set; } = default!;

        [Required(ErrorMessage = "SiteId zorunludur")]
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir SiteId girilmelidir.")]
        public int SiteId { get; set; }


    }
}