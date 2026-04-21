namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.YoneticiSiteEvents
{
    public record YoneticiSiteChangedEvent(int SiteId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public YoneticiSiteChangedEvent() : this(0)
        {
        }
    }
}
