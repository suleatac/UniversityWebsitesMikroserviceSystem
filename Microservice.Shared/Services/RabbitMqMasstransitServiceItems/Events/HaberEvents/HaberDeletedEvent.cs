namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.HaberEvents
{
    public record HaberDeletedEvent(int SiteId, int DilId);
}
