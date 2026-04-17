namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.HaberEvents
{
    public record HaberCreatedEvent(int SiteId, int DilId);
}
