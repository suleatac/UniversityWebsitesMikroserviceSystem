using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.CreateBandLogo
{
    public record CreateBandLogoCommand : IRequestByServiceResult<CreateBandLogoResponse>
    {
        public int SiteId { get; init; }
        public int DilId { get; init; }

        public string Ad { get; init; } = default!;
        public string ImgUrl { get; init; } = default!;
        public string? Link { get; init; }
    } 
}
