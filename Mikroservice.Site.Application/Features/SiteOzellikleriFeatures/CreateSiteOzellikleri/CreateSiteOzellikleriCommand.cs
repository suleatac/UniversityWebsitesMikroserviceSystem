using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.CreateSiteOzellikleri
{
    public record CreateSiteOzellikleriCommand : IRequestByServiceResult
    {
        public int SiteId { get; init; }
        public string SiteAdress { get; init; } = default!;
        public string SiteAdressEng { get; init; } = default!;
        public string SiteBaslangicHakkimizda { get; init; } = default!;
        public string SiteBaslangicHakkimizdaEng { get; init; } = default!;
        public string SiteTelNo { get; init; } = default!;
        public string SiteFaxNo { get; init; } = default!;
        public string SiteFacebookAdress { get; init; } = default!;
        public string SiteTwitterAdress { get; init; } = default!;
        public string SiteInstagramAdress { get; init; } = default!;
        public string SiteYoutubeAdress { get; init; } = default!;
        public string SiteHaritaAdress { get; init; } = default!;
        public string SiteBaslangicVideoLink { get; init; } = default!;
        public string SiteBaslangicVideoResimAdress { get; init; } = default!;
        public string SiteWatsappAdress { get; init; } = default!;
        public string SiteLinkedinAdress { get; init; } = default!;
        public string SiteHakkindaLink { get; init; } = default!;
        public string SiteVideoType { get; init; } = default!;
        public string SiteHakkindaResim { get; init; } = default!;
        public string SiteFooterLogo { get; init; } = default!;
        public string SiteTopbarLogo { get; init; } = default!;
    }
}
