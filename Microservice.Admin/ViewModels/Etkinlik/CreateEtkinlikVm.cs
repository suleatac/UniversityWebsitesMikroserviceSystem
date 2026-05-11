namespace Microservice.Admin.ViewModels.Etkinlik
{
    public class CreateEtkinlikVm
    {
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int? HedefId { get; set; }

        public string Baslik { get; set; } = default!;
        public string KisaAciklama { get; set; } = default!;
        public string IcerikMetni { get; set; } = default!;

        public string? Link { get; set; }
        public string? ResimUrl { get; set; }

        public DateTime YayimTarihi { get; set; } = DateTime.Now;
        public DateTime? BaslamaTarihi { get; set; }
        public DateTime? BitisTarihi { get; set; }

        public string? SeoUrl { get; set; }
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
    }
}