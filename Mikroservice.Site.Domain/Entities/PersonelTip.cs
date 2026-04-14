namespace Mikroservice.Site.Domain.Entities
{
    public class PersonelTip
    {
        public int Id { get; set; }
        public string Ad { get; set; }=default!;
        public ICollection<SitePersonel> SitePersonels { get; set; } = new List<SitePersonel>();
    }
}
