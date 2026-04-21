namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.MenuEvents
{
    public record MenuChangedEvent(int SiteId, int DilId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public MenuChangedEvent() : this(0, 0)
        {
        }
    }
}
