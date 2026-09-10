namespace Microservice.Admin.ViewModels.Menu
{
    // Mikroservice.Site.Domain.Enums.MenuLocation karsiligi (API'ye derleme bagimliligi olmadan).
    // Form dropdown'lari icin statik secenek listesi saglar.
    public static class MenuLocationOption
    {
        public const int Header = 1;
        public const int Footer = 2;
        public const int Sidebar = 3;

        public static List<(int Value, string Label)> All => new()
        {
            (Header, "Üst Menü (Header)"),
            (Footer, "Alt Menü - Hızlı Erişim (Footer)"),
            (Sidebar, "Kenar Çubuğu (Sidebar)")
        };

        public static string Label(int location) =>
            All.FirstOrDefault(x => x.Value == location).Label ?? "Bilinmiyor";
    }
}
