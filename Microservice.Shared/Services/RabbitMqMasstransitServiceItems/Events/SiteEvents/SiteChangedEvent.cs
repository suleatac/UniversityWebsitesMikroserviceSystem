namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SiteEvents
{
    public enum SiteChangeType
    {
        Created,
        Updated,
        Deleted
    }

    public record SiteChangedEvent(int SiteId, string SiteAlanAdi, string? PreviousSiteAlanAdi, SiteChangeType ChangeType)
    {
        public SiteChangedEvent() : this(0, string.Empty, null, SiteChangeType.Created)
        {
        }
    }
}
