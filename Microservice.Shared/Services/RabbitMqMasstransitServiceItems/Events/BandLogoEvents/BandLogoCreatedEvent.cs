namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BandLogoEvents
{
    public record BandLogoCreatedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public BandLogoCreatedEvent() : this(0, 0)
        {
        }
    }

}
