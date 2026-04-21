namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BilgiEvents
{
    public record BilgiCreatedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public BilgiCreatedEvent() : this(0, 0)
        {
        }
    }
}
