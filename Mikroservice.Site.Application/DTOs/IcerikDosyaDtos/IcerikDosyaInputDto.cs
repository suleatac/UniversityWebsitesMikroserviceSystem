namespace Mikroservice.Site.Application.DTOs.IcerikDosyaDtos
{
    /// <summary>
    /// Create/Update komutlarinda gonderilen ek dosya girdisi.
    /// Id degeri 0 ise yeni dosya, deger varsa mevcut dosyanin guncellemesi olarak islenir.
    /// </summary>
    public class IcerikDosyaInputDto
    {
        public int Id { get; set; }
        public string? Baslik { get; set; }
        public string DosyaUrl { get; set; } = default!;
        public string DosyaAdi { get; set; } = default!;
        public long DosyaBoyut { get; set; }
        public string? DosyaTuru { get; set; }
        public int Sira { get; set; }
    }
}
