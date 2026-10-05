namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SyncEvents
{
    // DIKKAT: Bu kayitlar Microservice.Shared'taki ile AYNI namespace ve type adina sahiptir.
    // MassTransit, mesaj rotalamasini CLR tipinin tam adindan (namespace + type) uretildigi icin
    // publish (Personel/Ogrenci Api) ve consume (Web) taraflarinin tam olarak ayni mesaj turu adini
    // kullanmasi gerekir. Web projemi Microservice.Shared'e referans vermedigi icin (Refit/surum
    // cakismasini onlemek adina) kopyalanmistir. Shared tarafinda degisiklik yapilirsa burada da
    // guncellenmelidir.

    /// <summary>
    /// Personel Hangfire senkronizasyonu basariyla tamamlandiginda yayinlanir (sinyal event'i).
    /// </summary>
    public record PersonelSyncCompletedEvent(int ProcessedCount, DateTime CompletedAt, string SourceService)
    {
        public PersonelSyncCompletedEvent() : this(0, default, "")
        {
        }
    }

    /// <summary>
    /// Ogrenci Hangfire senkronizasyonu basariyla tamamlandiginda yayinlanir (sinyal event'i).
    /// </summary>
    public record OgrenciSyncCompletedEvent(int ProcessedCount, DateTime CompletedAt, string SourceService)
    {
        public OgrenciSyncCompletedEvent() : this(0, default, "")
        {
        }
    }
}
