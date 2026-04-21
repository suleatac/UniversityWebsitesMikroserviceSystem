namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BandLogoEvents
{
    public record BandLogoUpdatedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public BandLogoUpdatedEvent() : this(0, 0)
        {
        }
    }
}
