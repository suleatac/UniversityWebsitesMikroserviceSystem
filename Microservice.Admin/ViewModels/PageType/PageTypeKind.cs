namespace Microservice.Admin.ViewModels.PageType
{
    // Mikroservice.Site.Domain.Enums.PageTypeKind ile aynı değerleri yansıtır
    // (Site API'ye derleme bağımlılığı olmadan).
    public enum PageTypeKind
    {
        Home = 1,
        Menu = 2,
        Haberler = 3,
        Haber = 4,
        Duyurular = 5,
        Duyuru = 6,
        Banner = 7,
        Bilgi = 8,
        Etkinlik = 9,
        Videolar = 10,
        Video = 11,
        StaticPage = 12,
    }
}
