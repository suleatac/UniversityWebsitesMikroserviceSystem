using Microservice.Web.ViewModels.Paged;
using Microservice.Web.ViewModels.Site;

namespace Microservice.Web.ViewModels.GaleriResim
{
    /// <summary>
    /// Galeri resmi liste sayfasi modeli: sayfali + kategori/arama filtreli liste.
    /// </summary>
    public class GaleriResimListPageViewModel
    {
        public SiteDetailGetVm Site { get; set; } = null!;

        public string LanguageCode { get; set; } = "tr";

        // Sayfa ici arama kelimesi (?q=...)
        public string? Query { get; set; }

        // Aktif kategori filtresi (?kategori=...)
        public string? Kategori { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 8;

        public PagedResultVm<GetGaleriResimVm> Results { get; set; } = new();

        // Filtre cabugu icin benzersiz kategori listesi
        public List<string> Kategoriler { get; set; } = new();

        public bool HasQuery => !string.IsNullOrWhiteSpace(Query);

        public bool HasKategori => !string.IsNullOrWhiteSpace(Kategori);
    }
}
