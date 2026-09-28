namespace Mikroservice.Site.Domain.Enums
{
    public enum IcerikTip
    {
        Haber = 1,
        Duyuru = 2,
        Bilgi = 3,
        Etkinlik = 4,
        Video = 5,
        Banner = 6,
        Menu = 7,
        GaleriResim = 8
    }

    public enum MenuLocation
    {
        Header = 1,
        Footer = 2
    }

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
        Search=15,
        GaleriResimListesi = 16,
        GaleriResimDetay = 17,
        SSS=18,
        EtkinlikListesi = 19
    }

    /// <summary>
    /// PageBlock icerik tipi. Template3 _PageBlockPartial.cshtml bu degerlere
    /// gore render eder. Admin'de secilen tip ile dogru alanlarin doldurulmasi
    /// beklenir (Video => VideoUrl, Text => Content, Carousel => Medias).
    /// </summary>
    public static class BlockContentType
    {
        public const string Text = "text";
        public const string Video = "video";
        public const string Image = "image";
        public const string Carousel = "carousel";

        public static readonly string[] All = [Text, Video, Image, Carousel];

        public static bool IsValid(string? contentType) =>
            All.Contains(contentType, StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Video barindiran blocklarin video kaynagi tipi.
    /// </summary>
    public static class VideoTuru
    {
        public const string YouTube = "YouTube";
        public const string Local = "Local";

        public static readonly string[] All = [YouTube, Local];

        public static bool IsValid(string? videoType) =>
            All.Contains(videoType, StringComparer.OrdinalIgnoreCase);
    }

}
