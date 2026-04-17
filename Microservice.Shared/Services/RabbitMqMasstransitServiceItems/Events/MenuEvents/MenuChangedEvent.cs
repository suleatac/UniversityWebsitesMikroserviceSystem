namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.MenuEvents
{
    public record MenuChangedEvent(int SiteId, int DilId);
}
