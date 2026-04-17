namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.DuyuruEvents
{
    public record DuyuruDeletedEvent(int SiteId, int DilId);
}
