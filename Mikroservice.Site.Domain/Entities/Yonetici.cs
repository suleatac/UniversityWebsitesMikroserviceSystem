namespace Mikroservice.Site.Domain.Entities
{
    public class Yonetici
    {
        public int Id { get; set; }
        public string ProfileImgYol { get; set; }=default!;
        public int PersonelId { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<YoneticiSite> YoneticiSites { get; set; } = new List<YoneticiSite>();
    }
}
