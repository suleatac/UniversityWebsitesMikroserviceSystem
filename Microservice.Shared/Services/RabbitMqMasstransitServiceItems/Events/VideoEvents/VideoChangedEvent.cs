namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.VideoEvents
{
    public record VideoChangedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public VideoChangedEvent() : this(0, 0)
        {
        }
    }

}
