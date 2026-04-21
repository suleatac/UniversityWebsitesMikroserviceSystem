namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.EtkinlikEvents
{
    public record EtkinlikDeletedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public EtkinlikDeletedEvent() : this(0, 0)
        {
        }
    }
}
