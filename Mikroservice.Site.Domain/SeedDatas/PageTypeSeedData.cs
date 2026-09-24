using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Domain.SeedDatas
{
    /// <summary>
    /// Microservice.Web / Views / Templates / Template1 altindaki view'lara
    /// (cshtml dosyalarina) karsilik gelen sayfa turlari.
    /// PageTypeSeedService tarafindan her template ve her dil icin birer
    /// PageType kaydi uretilir.
    ///
    /// ViewName'ler TemplateController.GetTemplateViewPath ile birebir ayni
    /// olmali; ornek: Home -> Index.cshtml, HaberDetay -> Haber.cshtml.
    /// </summary>
    public class PageTypeSeedData
    {
        public class PageTypeSeedDefinition
        {
            public PageTypeKind Kind { get; init; }
            public string ViewName { get; init; } = default!;
            public string SlugTR { get; init; } = default!;
            public string SlugEN { get; init; } = default!;
        }

        public static List<PageTypeSeedDefinition> Definitions => new()
        {
            new() { Kind = PageTypeKind.Home,               ViewName = "Index",              SlugTR = "anasayfa",    SlugEN = "home" },
            new() { Kind = PageTypeKind.Menu,               ViewName = "Menu",               SlugTR = "menu",        SlugEN = "menu" },
            new() { Kind = PageTypeKind.DuyuruListesi,      ViewName = "DuyuruListesi",      SlugTR = "duyurular",   SlugEN = "announcements" },
            new() { Kind = PageTypeKind.DuyuruDetay,        ViewName = "Duyuru",             SlugTR = "duyuru",      SlugEN = "announcement" },
            new() { Kind = PageTypeKind.HaberListesi,       ViewName = "HaberListesi",       SlugTR = "haberler",    SlugEN = "news" },
            new() { Kind = PageTypeKind.HaberDetay,         ViewName = "Haber",              SlugTR = "haber",       SlugEN = "news-detail" },
            new() { Kind = PageTypeKind.Etkinlik,           ViewName = "Etkinlik",           SlugTR = "etkinlik",    SlugEN = "event" },
            new() { Kind = PageTypeKind.Bilgi,              ViewName = "Bilgi",              SlugTR = "bilgi",       SlugEN = "information" },
            new() { Kind = PageTypeKind.PersonelListesi,    ViewName = "PersonelListesi",    SlugTR = "personeller", SlugEN = "staff" },
            new() { Kind = PageTypeKind.PersonelDetay,      ViewName = "Personel",           SlugTR = "personel",    SlugEN = "staff-member" },
            new() { Kind = PageTypeKind.GaleriResimListesi, ViewName = "GaleriResimListesi", SlugTR = "galeri",      SlugEN = "gallery" },
            new() { Kind = PageTypeKind.GaleriResimDetay,   ViewName = "GaleriResmi",        SlugTR = "galeri-resim",SlugEN = "gallery-image" },
            new() { Kind = PageTypeKind.Search,             ViewName = "Search",             SlugTR = "arama",       SlugEN = "search" },
            new() { Kind = PageTypeKind.StaticPage,         ViewName = "Iletisim",           SlugTR = "iletisim",    SlugEN = "contact" },
            new() { Kind = PageTypeKind.Banner,             ViewName = "Banner",             SlugTR = "banner",      SlugEN = "banner" },
            new() { Kind = PageTypeKind.VideoListesi,       ViewName = "VideoListesi",       SlugTR = "videolar",    SlugEN = "videos" },
            new() { Kind = PageTypeKind.VideoDetay,         ViewName = "Video",              SlugTR = "video",       SlugEN = "video" },
        };

        public static List<PageTypeSeedDefinition> GetPageTypeSeedDefinitions()
        {
            return Definitions;
        }
    }
}
