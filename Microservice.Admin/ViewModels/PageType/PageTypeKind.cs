namespace Microservice.Admin.ViewModels.PageType
{
    // Mikroservice.Site.Domain.Enums.PageTypeKind ile aynı değerleri yansıtır
    // (Site API'ye derleme bağımlılığı olmadan).
    public enum PageTypeKind
    {
        Home = 1,
        Menu = 2,
        NewsList = 3,
        NewsDetail = 4,
        AnnouncementList = 5,
        AnnouncementDetail = 6,
        Banner = 7,
        Bilgi = 8,
        Etkinlik = 9,
        VideoList = 10,
        VideoDetail = 11,
        StaticPage = 12,
    }
}
