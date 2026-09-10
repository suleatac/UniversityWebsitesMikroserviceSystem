using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.SiteOzellikleri
{
    // Mikroservice.Site.Application.Features.SiteOzellikleriFeatures (Create/Update) CommandValidation
    // kurallariyla paralel tutulmustur (istemci tarafi on dogrulama).
    public class SiteOzellikleriVm
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "SiteId 0'dan büyük olmalıdır.")]
        public int SiteId { get; set; }
        public string? SiteAdress { get; set; } 
        public string? SiteAdressEng { get; set; } 
        public string? SiteBaslangicHakkimizda { get; set; } 
        public string? SiteBaslangicHakkimizdaEng { get; set; } 
        public string? SiteTelNo { get; set; } 
        public string? SiteFaxNo { get; set; } 
        public string? SiteFacebookAdress { get; set; } 
        public string? SiteTwitterAdress { get; set; } 
        public string? SiteInstagramAdress { get; set; } 
        public string? SiteYoutubeAdress { get; set; } 
        public string? SiteHaritaAdress { get; set; } 
        public string? SiteBaslangicVideoLink { get; set; } 
        public string? SiteBaslangicVideoResimAdress { get; set; } 
        public string? SiteVideoType { get; set; } 
        public string? SiteWatsappAdress { get; set; } 
        public string? SiteLinkedinAdress { get; set; } 
        public string? SiteHakkindaLink { get; set; } 
        public string? SiteHakkindaResim { get; set; } 
        public string? SiteFooterLogo { get; set; } 
        public string? SiteTopbarLogo { get; set; } 
    }
}