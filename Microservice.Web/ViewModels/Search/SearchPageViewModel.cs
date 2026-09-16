using Microservice.Web.ViewModels.Icerik;
using Microservice.Web.ViewModels.Paged;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Search
{
    public class SearchPageViewModel
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Arama kelimesi (?q=...)
        public string? Query { get; set; }

        // Icerik tipi filtresi (?tip=...) 1=Haber,2=Duyuru,3=Bilgi,4=Etkinlik,5=Video; null = hepsi
        public int? Tip { get; set; }

        public int Page { get; set; } = 1;

        public PagedResultVm<IcerikSearchVm> Results { get; set; } = new();

        public bool HasQuery => !string.IsNullOrWhiteSpace(Query);

        public string TypeLabel(int tip) => tip switch {
            1 => "Haber",
            2 => "Duyuru",
            3 => "Bilgi",
            4 => "Etkinlik",
            5 => "Video",
            _ => "Icerik"
        };
    }
}
