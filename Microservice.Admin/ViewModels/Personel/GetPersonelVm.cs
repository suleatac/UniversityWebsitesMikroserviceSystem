namespace Microservice.Admin.ViewModels.Personel
{
    public class GetPersonelVm
    {
        public int Id { get; set; }
        public string? Adi { get; set; }
        public string? Soyadi { get; set; }
        public string? Eposta { get; set; }
        public string? Username { get; set; }
        public string? PersonBase64Image { get; set; }
        public string? AsliUnvan { get; set; }
        public string? PersonEncryptedId { get; set; }
        public string? KadroUstBirim { get; set; }
        public string? KadroBirimi { get; set; }
    }
}
