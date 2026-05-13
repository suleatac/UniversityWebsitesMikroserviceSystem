using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.YoneticiSite
{
    public class YoneticiSiteVm
    {

        [Required(ErrorMessage = "PersonelId zorunludur")]
        public string KeycloakUserId { get; set; } = default!;

        [Required(ErrorMessage = "SiteId zorunludur")]
        public int SiteId { get; set; }


    }
}