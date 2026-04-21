namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.HaberEvents
{
    public record HaberUpdatedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public HaberUpdatedEvent() : this(0, 0)
        {
        }
    }
}
