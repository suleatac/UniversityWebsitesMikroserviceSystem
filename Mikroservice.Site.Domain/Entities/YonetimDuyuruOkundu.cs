namespace Microservice.Site.Domain.Entities
{
    public class YonetimDuyuruOkundu
    {
        public int Id { get; set; }
        public int YonetimDuyuruId { get; set; }
        public string KeycloakUserId { get; set; } = default!;
        public DateTime OkunmaTarihi { get; set; }

        // Navigation property
        public YonetimDuyuru YonetimDuyuru { get; set; } = default!;
    }
}
