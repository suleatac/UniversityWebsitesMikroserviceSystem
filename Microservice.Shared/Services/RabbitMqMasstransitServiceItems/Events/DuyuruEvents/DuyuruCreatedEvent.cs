namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.DuyuruEvents
{
    public record DuyuruCreatedEvent(int SiteId, int DilId);
}
