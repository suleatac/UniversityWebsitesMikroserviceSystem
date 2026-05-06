using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SiteFeatures.CreateSite
{
    public record CreateSiteCommand : IRequestByServiceResult<CreateSiteResponse>
    {
        public string SiteAdi { get; init; } = default!;
        public string SiteAdiEng { get; init; } = default!;
        public string SiteUrl { get; init; } = default!;
        public int BirimId { get; init; }
        public string SiteAlanAdi { get; init; } = default!;

        public string SiteEPosta { get; init; } = default!;
        public string SiteEPostaSifre { get; init; } = default!;
        public string SiteEPostaHost { get; init; } = default!;
        public int SiteEPostaPort { get; init; }

        public int TemplateId { get; init; }
    }
}
