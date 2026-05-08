namespace Microservice.Admin.ViewModels.YonetimDuyuru
{
    public class YonetimDuyuruVm
    {
        public int Id { get; set; }
        public string Baslik { get; set; } = default!;
        public string Icerik { get; set; } = default!;
        public DateTime EklenmeTarihi { get; set; }
    }
}
