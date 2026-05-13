using Microservice.Admin.ViewModels.Dil;
using Microservice.Admin.ViewModels.Site;

namespace Microservice.Admin.ViewModels.SiteSelection
{
    public class SiteSelectionVm
    {
        public List<SiteGetVm> Sites { get; set; } = new();
        public List<GetDilVm> Diller { get; set; } = new();
        public int CurrentSiteId { get; set; }
        public int CurrentDilId { get; set; } = 1;
        public string CurrentSiteName { get; set; } = "Site Seç";
        public string CurrentDilName { get; set; } = "Türkçe";
        public bool IsAdmin { get; set; }
        public List<int> AuthorizedSiteIds { get; set; } = new();
        /// <summary>
        /// Sistemde hiç site yoksa true olur. Admin kullanıcıyı site eklemeye yönlendirmek için kullanılır.
        /// </summary>
        public bool NoSitesAvailable { get; set; }
        /// <summary>
        /// Admin kullanıcı henüz site seçmemişse true olur. Site seçimini zorlamak için kullanılır.
        /// </summary>
        public bool MustSelectSite { get; set; }
    }
}