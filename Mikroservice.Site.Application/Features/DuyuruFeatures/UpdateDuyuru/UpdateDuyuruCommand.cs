using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.IcerikDosyaDtos;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.UpdateDuyuru
{
    public record UpdateDuyuruCommand : IRequestByServiceResult
    {
        // Icerige bagli ek dosyalar (istege bagli, istenilen kadar)
        public List<IcerikDosyaInputDto> Dosyalar { get; init; } = new();

        public int Id { get; init; }
        public int SiteId { get; init; }
        public int PageTypeId { get; init; }
        public int DilId { get; init; }
        public int HedefId { get; init; }

        public string Baslik { get; init; } = default!;
        public string? KisaAciklama { get; init; }
        public string? IcerikMetni { get; init; }

        public string? Link { get; init; }
        public string? ResimUrl { get; init; }

        public DateTime YayimTarihi { get; init; }
        public DateTime? BaslamaTarihi { get; init; }
        public DateTime? BitisTarihi { get; init; }

        // SEO (opsiyonel ama önemli)
        public string SeoUrl { get; init; } = default!;
        public string? SeoTitle { get; init; }
        public string? SeoDescription { get; init; }
    }
}
