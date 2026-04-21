namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.DuyuruEvents
{
    public record DuyuruDeletedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public DuyuruDeletedEvent() : this(0, 0)
        {
        }
    }
}
