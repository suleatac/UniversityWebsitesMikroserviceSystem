using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.UpdateSitePersonel
{
    public record UpdateSitePersonelCommand : IRequestByServiceResult
    {
        public int Id { get; set; }
        public int SiteId { get; init; }
        public int PersonelId { get; init; }
        public int UnvanId { get; init; }
        public int PersonelTipId { get; init; }

        public string? ResimUrl { get; init; } 
        public string? IlgiAlanlari { get; init; }
        public string? BlogAdress { get; init; } 
        public string? TwitterAdress { get; init; } 
        public string? FacebookAdress { get; init; } 
        public string? InstagramAdress { get; init; } 
        public string? GoogleplusAdress { get; init; } 

        public string? Hakkinda { get; init; } 
        public string? DeneyimVeCalismalari { get; init; } 
    }
}
