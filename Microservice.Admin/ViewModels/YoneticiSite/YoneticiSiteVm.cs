using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.YoneticiSite
{
    public class YoneticiSiteVm
    {

        [Required(ErrorMessage = "PersonelId zorunludur")]
        public int PersonelId { get; set; } = default!;

        [Required(ErrorMessage = "SiteId zorunludur")]
        public int SiteId { get; set; }

        [Required(ErrorMessage = "YoneticiTipiId zorunludur")]
        public int YoneticiTipiId { get; set; }
    }
}