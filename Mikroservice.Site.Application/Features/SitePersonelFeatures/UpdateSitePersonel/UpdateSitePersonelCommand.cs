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

        public string ResimUrl { get; init; } = default!;
        public string IlgiAlanlari { get; init; } = default!;
        public string BlogAdress { get; init; } = default!;
        public string TwitterAdress { get; init; } = default!;
        public string FacebookAdress { get; init; } = default!;
        public string InstagramAdress { get; init; } = default!;
        public string GoogleplusAdress { get; init; } = default!;

        public string Hakkinda { get; init; } = default!;
        public string DeneyimVeCalismalari { get; init; } = default!;
    }
}
