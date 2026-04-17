namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.EtkinlikEvents
{
    public record EtkinlikCreatedEvent(int SiteId, int DilId);
}
