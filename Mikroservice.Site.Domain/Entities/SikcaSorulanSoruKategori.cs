namespace Mikroservice.Site.Domain.Entities
{
    public class SikcaSorulanSoruKategori
    {
        public int Id { get; set; }
        public string Ad { get; set; } = default!;
        public int Sira { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<SikcaSorulanSoru> SikcaSorulanSorus { get; set; } = new List<SikcaSorulanSoru>();
    }
}
