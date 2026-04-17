namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BannerEvents
{
    public record BannerCreatedEvent(int SiteId, int DilId);
}
