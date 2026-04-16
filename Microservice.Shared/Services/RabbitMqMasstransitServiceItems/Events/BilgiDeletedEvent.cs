namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events
{
    public record BilgiDeletedEvent(int SiteId, int DilId);
}
