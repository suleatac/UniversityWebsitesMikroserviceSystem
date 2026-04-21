namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.DuyuruEvents
{
    public record DuyuruCreatedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public DuyuruCreatedEvent() : this(0, 0)
        {
        }
    }
}
