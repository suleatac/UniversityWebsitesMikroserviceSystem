namespace Mikroservice.Site.Domain.Entities
{
    public class YoneticiSite
    {
        public int Id { get; set; }

        public string KeycloakUserId { get; set; } = default!;

        public int SiteId { get; set; }

        public bool IsDeleted { get; set; }
        public Site Site { get; set; } = default!;
    }
}
