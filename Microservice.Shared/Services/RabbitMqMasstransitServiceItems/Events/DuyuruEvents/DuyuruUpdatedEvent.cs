namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.DuyuruEvents
{
    public record DuyuruUpdatedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public DuyuruUpdatedEvent() : this(0, 0)
        {
        }
    }
}
