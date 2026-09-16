using Microservice.Web.ViewModels.Paged;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.Template
{
    /// <summary>
    /// Haber/Duyuru gibi icerik listesi sayfalarinin sayfali + arama destekli model.
    /// </summary>
    public class ContentListPageViewModel<T>
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Sayfa ici arama kelimesi (?q=...)
        public string? Query { get; set; }

        public int Page { get; set; } = 1;

        public PagedResultVm<T> Results { get; set; } = new();

        public bool HasQuery => !string.IsNullOrWhiteSpace(Query);
    }
}
