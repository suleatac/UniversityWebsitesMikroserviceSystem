namespace Mikroservice.Site.Domain.Entities
{
    public class ShortcutButton
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public int HedefId { get; set; }
        public string Ad { get; set; } = default!;
        public string? Link { get; set; } = default!;
        public string? IconUrl { get; set; }
        public string? ImageUrl { get; set; }
        public bool? IsIconImage { get; set; }
        public int Sira { get; set; }
        public DateTime OlusturulmaTarihi { get; set; }
        public bool IsDeleted { get; set; }


        // 🔥 NAVIGATION Mikroservice.Site.Domain içinde Entities klasöründe HızlıErisim entity'si ekledim. Onun için uygun configürasyonu, repository'leri ve Apileri hazırla. 
        public Site Site { get; set; } = default!;
        public Dil Dil { get; set; } = default!;
        public Hedef Hedef { get; set; } = default!;
    }
}
