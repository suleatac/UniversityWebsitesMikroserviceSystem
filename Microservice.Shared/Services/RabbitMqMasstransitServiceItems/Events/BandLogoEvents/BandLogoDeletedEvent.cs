namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BandLogoEvents
{
    public record BandLogoDeletedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public BandLogoDeletedEvent() : this(0, 0)
        {
        }
    }

}
