using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SiteOzellikleriFeatures.CreateSiteOzellikleri
{
    public record CreateSiteOzellikleriCommand : IRequestByServiceResult<CreateSiteOzellikleriResponse>
    {
        public int SiteId { get; init; }

        public string? SiteAdress { get; init; } 
        public string? SiteAdressEng { get; init; } 

        public string? SiteBaslangicHakkimizda { get; init; } 
        public string? SiteBaslangicHakkimizdaEng { get; init; } 

        public string? SiteTelNo { get; init; } 
        public string? SiteFaxNo { get; init; } 

        public string? SiteFacebookAdress { get; init; } 
        public string? SiteTwitterAdress { get; init; } 
        public string? SiteInstagramAdress { get; init; } 
        public string? SiteYoutubeAdress { get; init; } 
        public string? SiteLinkedinAdress { get; init; } 

        public string? SiteHaritaAdress { get; init; } 

        public string? SiteBaslangicVideoLink { get; init; } 
        public string? SiteBaslangicVideoResimAdress { get; init; } 
        public string? SiteVideoType { get; init; } 

        public string? SiteWatsappAdress { get; init; } 

        public string? SiteHakkindaLink { get; init; } 
        public string? SiteHakkindaResim { get; init; } 

        public string? SiteFooterLogo { get; init; } 
        public string? SiteTopbarLogo { get; init; } 
    }
}
