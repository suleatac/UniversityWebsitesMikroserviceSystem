namespace Microservice.Admin.ViewModels.YonetimDuyuru
{
    public class YonetimDuyuruDetailVm
    {
        public int Id { get; set; }
        public string Baslik { get; set; } = default!;
        public string Icerik { get; set; } = default!;
        public DateTime EklenmeTarihi { get; set; }
        public bool OkunduMu { get; set; }
    }
}
