namespace Mikroservice.Site.Domain.Entities
{
    public class Dil
    {
        public int Id { get; set; }
        public string Ad { get; set; } = default!;              // Türkçe, İngilizce, Almanca
        public string InternationalAd { get; set; } = default!; // English, German, etc.
        public string Kod { get; set; } = default!;             // tr, en, de
        public bool IsActive { get; set; } = true;

    }
}
