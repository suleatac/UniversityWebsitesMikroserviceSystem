namespace Microservice.Admin.ViewModels.PageType
{
    // Mikroservice.Site.Domain.Enums.PageTypeKind ile aynı değerleri yansıtır
    // (Site API'ye derleme bağımlılığı olmadan).
    public enum PageTypeKind
    {
        Home = 1,
        Menu = 2,
        HaberListesi = 3,
        HaberDetay = 4,
        PersonelListesi = 5,
        PersonelDetay = 6,
        DuyuruListesi = 7,
        DuyuruDetay = 8,
        Banner = 9,
        Bilgi = 10,
        Etkinlik = 11,
        VideoListesi = 12,
        VideoDetay = 13,
        StaticPage = 14,
        Search = 15
    }
}
