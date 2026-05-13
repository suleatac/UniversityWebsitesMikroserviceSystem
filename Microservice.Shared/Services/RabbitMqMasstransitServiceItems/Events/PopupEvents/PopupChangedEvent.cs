namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.PopupEvents
{
    public record PopupChangedEvent(int SiteId)
    {
        // MassTransit'in ihtiyacı olan boş constructor
        public PopupChangedEvent() : this(0)
        {
        }
    }
}
