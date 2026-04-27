namespace Mikroservice.Site.Application.DTOs.SiteDtos
{
    public class SiteDto
    {
        public int Id { get; set; }
        public string SiteAdi { get; set; }= default!;
        public string SiteUrl { get; set; }= default!;
        public string SiteEPosta { get; set; }=default!;
    }
}
