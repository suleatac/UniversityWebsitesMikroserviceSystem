namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SitePersonelEvents
{
    public record SitePersonelChangedEvent(int SiteId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public SitePersonelChangedEvent() : this(0)
        {
        }
    }
}
