namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events
{
    public record BannerDeletedEvent(int SiteId, int DilId);
}
