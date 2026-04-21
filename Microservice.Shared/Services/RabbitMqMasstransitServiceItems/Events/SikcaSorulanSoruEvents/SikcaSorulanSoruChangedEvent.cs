namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruEvents
{
    namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SikcaSorulanSoruEvents
    {
        public record SikcaSorulanSoruChangedEvent(int SiteId, int DilId)
        {
            // MassTransit'in ihtiyacı olan boş constructor
            public SikcaSorulanSoruChangedEvent() : this(0, 0)
            {
            }
        }
    }
}
