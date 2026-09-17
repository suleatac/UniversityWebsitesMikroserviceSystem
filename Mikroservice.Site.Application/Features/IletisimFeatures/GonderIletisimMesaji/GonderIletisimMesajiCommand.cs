using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.IletisimFeatures.GonderIletisimMesaji
{
    /// <summary>
    /// Iletisim formu icerigi. SiteId'ye karsilik gelen sitenin SMTP bilgileriyle
    /// yine sitenin kendi e-posta adresine gonderilmek uzere RabbitMQ'ya basilir.
    /// </summary>
    public record GonderIletisimMesajiCommand : IRequestByServiceResult
    {
        public int SiteId { get; init; }

        public string AdSoyad { get; init; } = default!;
        public string Eposta { get; init; } = default!;
        public string Konu { get; init; } = default!;
        public string Mesaj { get; init; } = default!;
        public string? Telefon { get; init; }
    }
}
