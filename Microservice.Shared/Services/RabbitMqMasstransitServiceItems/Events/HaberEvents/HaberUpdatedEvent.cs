namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.HaberEvents
{
    public record HaberUpdatedEvent(int SiteId, int DilId);
}
