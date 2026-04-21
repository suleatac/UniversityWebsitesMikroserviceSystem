namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BilgiEvents
{
    public record BilgiDeletedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public BilgiDeletedEvent() : this(0, 0)
        {
        }
    }
}
