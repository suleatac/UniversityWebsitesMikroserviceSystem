namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BannerEvents
{
    public record BannerDeletedEvent(int SiteId, int DilId);
}
