namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SyncEvents
{
    /// <summary>
    /// Personel Hangfire senkronizasyonu basariyla tamamlandiginda yayinlanir.
    /// Web tarafi bu event ile Redis cache'ini duser ve SignalR uzerinden sayfaları bilgilendirir.
    /// Sinyal amacli event'dir; veri govdesi tasimaz.
    /// </summary>
    public record PersonelSyncCompletedEvent(int ProcessedCount, DateTime CompletedAt, string SourceService)
    {
        // MassTransit'in ihtiyaci olan bos constructor
        public PersonelSyncCompletedEvent() : this(0, default, "")
        {
        }
    }
}
