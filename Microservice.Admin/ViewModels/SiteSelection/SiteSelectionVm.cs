using Microservice.Admin.ViewModels.Dil;
using Microservice.Admin.ViewModels.Site;

namespace Microservice.Admin.ViewModels.SiteSelection
{
    public class SiteSelectionVm
    {
        public List<SiteGetVm> Sites { get; set; } = new();
        public List<GetDilVm> Diller { get; set; } = new();
        public int CurrentSiteId { get; set; } = 1;
        public int CurrentDilId { get; set; } = 1;
        public string CurrentSiteName { get; set; } = "Site Seç";
        public string CurrentDilName { get; set; } = "Türkçe";
    }
}