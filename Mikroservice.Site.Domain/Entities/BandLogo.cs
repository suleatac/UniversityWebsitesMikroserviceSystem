namespace Mikroservice.Site.Domain.Entities
{
    public class BandLogo
    {
        public int Id { get; set; }

        public int SiteId { get; set; }
        public int DilId { get; set; }

        public string Ad { get; set; } = default!;
        public string ImgUrl { get; set; } = default!;
        public string? Link { get; set; }

        public DateTime EklenmeTarihi { get; set; }

        public Site Site { get; set; } = default!;
        public Dil? Dil { get; set; }
    }
}
