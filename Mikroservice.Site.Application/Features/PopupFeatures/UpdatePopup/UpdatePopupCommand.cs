using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.PopupFeatures.UpdatePopup
{
    public record UpdatePopupCommand : IRequestByServiceResult
    {
        public int Id { get; init; }
        public int SiteId { get; init; }
        public int DilId { get; set; }
        public string Baslik { get; init; } = default!;
        public string? KisaAciklama { get; init; }
        public string? Link { get; init; }
        public string? ResimUrl { get; init; }
        public int GosterimSayisi { get; init; }
        public DateTime YayimTarihi { get; init; }
        public DateTime? BaslamaTarihi { get; init; }
        public DateTime? BitisTarihi { get; init; }
        public bool TamEkranMi { get; init; } = false;
        public int GosterimSuresiSaniye { get; init; }
        public bool CookieIleTekrarGosterme { get; init; } = true;
    }
}
