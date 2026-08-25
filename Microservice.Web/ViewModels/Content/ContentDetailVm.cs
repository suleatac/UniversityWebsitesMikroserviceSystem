namespace Microservice.Web.ViewModels.Content
{
    public class ContentDetailVm
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public string Baslik { get; set; } = string.Empty;
        public string KisaAciklama { get; set; } = string.Empty;
        public string IcerikMetni { get; set; } = string.Empty;
        public string? Link { get; set; }
        public string? ResimUrl { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime YayimTarihi { get; set; }
        public DateTime? BaslamaTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }
        public string? SeoUrl { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
    }
}
