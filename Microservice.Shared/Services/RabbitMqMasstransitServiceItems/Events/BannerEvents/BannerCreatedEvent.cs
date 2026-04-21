namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BannerEvents
{
    public record BannerCreatedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public BannerCreatedEvent() : this(0, 0)
        {
        }
    }
}
