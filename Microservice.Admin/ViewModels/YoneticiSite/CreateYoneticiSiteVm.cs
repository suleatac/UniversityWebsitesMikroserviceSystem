using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.YoneticiSite
{
    public class CreateYoneticiSiteVm
    {
        [Required(ErrorMessage = "Keycloak UserId zorunludur")]
        public string KeycloakUserId { get; set; } = default!;

        [Required(ErrorMessage = "SiteId zorunludur")]
        public int SiteId { get; set; }

        [Required(ErrorMessage = "YoneticiTipiId zorunludur")]
        public int YoneticiTipiId { get; set; }
    }
}