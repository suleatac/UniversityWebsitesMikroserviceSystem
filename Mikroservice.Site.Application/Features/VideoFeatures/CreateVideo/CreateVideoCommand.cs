using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.VideoFeatures.CreateVideo
{
    public record CreateVideoCommand : IRequestByServiceResult<CreateVideoResponse>
    {
        public int SiteId { get; init; }
        public int DilId { get; init; }
        public int? HedefId { get; init; }

        public string Baslik { get; init; } = default!;
        public string KisaAciklama { get; init; } = default!;
        public string IcerikMetni { get; init; } = default!;

        public string? Link { get; init; }
        public string? ResimUrl { get; init; }

        public DateTime YayimTarihi { get; init; }
        public DateTime EklemeTarihi { get; init; }

        public DateTime? BaslamaTarihi { get; init; }
        public DateTime? BitisTarihi { get; init; }

        public string? SeoUrl { get; init; }
        public string? SeoTitle { get; init; }
        public string? SeoDescription { get; init; }

        public string? VideoUrl { get; init; }
    }
}