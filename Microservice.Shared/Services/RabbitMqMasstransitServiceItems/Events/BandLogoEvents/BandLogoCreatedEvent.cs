namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BandLogoEvents
{
    public record BandLogoCreatedEvent(int SiteId, int DilId);
}
