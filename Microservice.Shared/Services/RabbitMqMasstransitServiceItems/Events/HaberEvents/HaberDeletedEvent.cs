namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.HaberEvents
{
    public record HaberDeletedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public HaberDeletedEvent() : this(0, 0)
        {
        }
    }
}
