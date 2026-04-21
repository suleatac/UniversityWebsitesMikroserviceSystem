namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.EtkinlikEvents
{
    public record EtkinlikCreatedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public EtkinlikCreatedEvent() : this(0, 0)
        {
        }
    }
}
