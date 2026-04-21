namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.EtkinlikEvents
{
    public record EtkinlikUpdatedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public EtkinlikUpdatedEvent() : this(0, 0)
        {
        }
    }
}
