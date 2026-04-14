namespace Mikroservice.Site.Domain.Entities
{
    public class Unvan
    {
        public int Id { get; set; }
        public int Tip { get; set; }
        public string Ad { get; set; }=default!;
        public string KisaAd { get; set; }=default!;
        public int Sira { get; set; }

        public ICollection<SitePersonel> SitePersonels { get; set; } = new List<SitePersonel>();
    }
}
