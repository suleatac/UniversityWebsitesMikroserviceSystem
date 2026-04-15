namespace Mikroservice.Site.Domain.Entities
{
    public class PersonelTip
    {
        public int Id { get; set; }
        public string Ad { get; set; }=default!;
        public ICollection<SitePersonel> SitePersonels { get; set; } = new List<SitePersonel>();
        public ICollection<Unvan> Unvans { get; set; } = new List<Unvan>();
    }
}
