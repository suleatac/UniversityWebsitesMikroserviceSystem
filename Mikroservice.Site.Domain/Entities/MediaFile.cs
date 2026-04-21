namespace Mikroservice.Site.Domain.Entities
{
    public class MediaFile
    {
        public int Id { get; set; }
        public int SiteId { get; set; }
        public int DilId { get; set; }
        public string Path { get; set; } = default!;
        public string Url { get; set; } = default!;
        public DateTime CreatedAt { get; set; }
        public Dil Dil { get; set; } = default!;
        public Site Site { get; set; } = default!;
    }
}
