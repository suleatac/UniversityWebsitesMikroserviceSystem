namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BannerEvents
{
    public record BannerUpdatedEvent(int SiteId, int DilId);
}
