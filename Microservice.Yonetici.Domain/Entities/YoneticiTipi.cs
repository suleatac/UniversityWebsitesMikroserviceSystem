namespace Microservice.Yonetici.Domain.Entities
{
    public class YoneticiTipi
    {
        public int Id { get; set; }
        public string TipAdi { get; set; } = default!;
        public int Value { get; set; }
    }
}
