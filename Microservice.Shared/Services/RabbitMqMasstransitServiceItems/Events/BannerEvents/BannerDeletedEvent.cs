namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BannerEvents
{
    public record BannerDeletedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public BannerDeletedEvent() : this(0, 0)
        {
        }
    }
}
