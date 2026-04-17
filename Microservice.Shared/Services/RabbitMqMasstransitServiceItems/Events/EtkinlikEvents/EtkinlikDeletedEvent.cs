namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.EtkinlikEvents
{
    public record EtkinlikDeletedEvent(int SiteId, int DilId);
}
