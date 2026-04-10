namespace Microservice.Yonetici.Domain.Entities
{
    public class YoneticiDuyuru
    {
        public int Id { get; set; }
        public string Baslik { get; set; }= default!;
        public string Icerik { get; set; }= default!;
        public DateTime EklenmeTarihi { get; set; }
        public bool Aktif { get; set; }
    }
}
