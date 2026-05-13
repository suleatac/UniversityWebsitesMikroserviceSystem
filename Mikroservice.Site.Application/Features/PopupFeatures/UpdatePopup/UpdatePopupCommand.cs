using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.PopupFeatures.UpdatePopup
{
    public record UpdatePopupCommand : IRequestByServiceResult
    {
        public int Id { get; init; }
        public int SiteId { get; init; }
        public string Baslik { get; init; } = default!;
        public string KisaAciklama { get; init; } = default!;
        public string IcerikMetni { get; init; } = default!;
        public string? Link { get; init; }
        public string? ResimUrl { get; init; }
        public int GosterimSayisi { get; init; }
        public DateTime YayimTarihi { get; init; }
        public DateTime? BaslamaTarihi { get; init; }
        public DateTime? BitisTarihi { get; init; }
        public string? SeoUrl { get; init; }
        public string? SeoTitle { get; init; }
        public string? SeoDescription { get; init; }
        public bool TamEkranMi { get; init; } = false;
        public int GosterimSuresiSaniye { get; init; }
        public bool CookieIleTekrarGosterme { get; init; } = true;
    }
}
