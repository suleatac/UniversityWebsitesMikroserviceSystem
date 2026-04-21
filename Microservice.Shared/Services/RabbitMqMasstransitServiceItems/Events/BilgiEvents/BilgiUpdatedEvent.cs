namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BilgiEvents
{
    public record BilgiUpdatedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public BilgiUpdatedEvent() : this(0, 0)
        {
        }
    }
}
