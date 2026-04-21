namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SiteOzellikleriEvents
{
    public record SiteOzellikleriChangedEvent(int SiteId)
    {
        public SiteOzellikleriChangedEvent()
            : this(0)
        {
        }

    }
}
