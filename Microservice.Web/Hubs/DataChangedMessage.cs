namespace Microservice.Web.Hubs
{
    /// <summary>
    /// "DataChanged" olayinin tip-güvenli payload'u.
    /// SignalR JSON protokolü default olarak camelCase serileştirir;
    /// bu yüzden JS tarafinda msg.scope / msg.processedCount / msg.completedAt olarak okunur.
    /// </summary>
    /// <param name="Scope">Degisen veri kapsami: "personel" | "ogrenci".</param>
    /// <param name="ProcessedCount">Senkronizasyonda islenen kayit sayisi.</param>
    /// <param name="CompletedAt">Senkronizasyonun tamamlanma zamani (UTC).</param>
    public record DataChangedMessage(string Scope, int ProcessedCount, DateTime CompletedAt);
}
