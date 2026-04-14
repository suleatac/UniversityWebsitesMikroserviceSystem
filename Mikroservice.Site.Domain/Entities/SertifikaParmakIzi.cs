namespace Mikroservice.Site.Domain.Entities
{
    public class SertifikaParmakIzi
    {
        public int Id { get; set; }
        public string SertifikaParmakIziNumarasi { get; set; }= default!;
        public DateTime? SertifikaYili { get; set; }
        public bool Aktif { get; set; }
        public ICollection<Site> Sites { get; set; } = new List<Site>();
    }
}
