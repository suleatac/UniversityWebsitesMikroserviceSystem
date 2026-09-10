using System.ComponentModel.DataAnnotations;

namespace Microservice.Admin.ViewModels.SitePersonel
{
    // Mikroservice.Site.Application.Features.SitePersonelFeatures.CreateSitePersonel.CreateSitePersonelCommandValidation
    // kurallariyla paralel tutulmustur (istemci tarafi on dogrulama).
    public class CreateSitePersonelVm
    {
        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir SiteId girilmelidir.")]
        public int SiteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir PersonelId girilmelidir.")]
        public int PersonelId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir UnvanId girilmelidir.")]
        public int UnvanId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir PersonelTipId girilmelidir.")]
        public int PersonelTipId { get; set; }

        public string? ResimUrl { get; set; } 
        public string? IlgiAlanlari { get; set; } 
        public string? BlogAdress { get; set; } 
        public string? TwitterAdress { get; set; } 
        public string? FacebookAdress { get; set; } 
        public string? InstagramAdress { get; set; } 
        public string? GoogleplusAdress { get; set; } 
        public string? Hakkinda { get; set; } 
        public string? DeneyimVeCalismalari { get; set; } 
    }
}
