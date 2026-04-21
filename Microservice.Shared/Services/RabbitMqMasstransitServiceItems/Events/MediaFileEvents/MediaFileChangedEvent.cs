namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.MediaFileEvents
{
    public record MediaFileChangedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public MediaFileChangedEvent() : this(0, 0)
        {
        }
    }

}
