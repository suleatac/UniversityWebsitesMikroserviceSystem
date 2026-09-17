namespace Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.IletisimEvents
{
    /// <summary>
    /// Iletisim formundan gelen mesaj. Site API publish eder,
    /// tuketici taraf sitenin kendi SMTP bilgileriyle maili site e-postasina gonderir.
    /// </summary>
    public record IletisimMesajiEvent(
        int SiteId,
        string SiteAdi,
        string AdSoyad,
        string Eposta,
        string Konu,
        string Mesaj,
        string? Telefon,
        DateTime GonderimZamani)
    {
        public IletisimMesajiEvent() : this(0, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, null, default)
        {
        }
    }
}
