using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.UpdateBandLogo
{
    public class UpdateBandLogoCommand : IRequestByServiceResult
    {
        public int Id { get; init; }
        public int SiteId { get; init; }
        public int DilId { get; init; }

        public string Ad { get; init; } = default!;
        public string ImgUrl { get; init; } = default!;
        public string? Link { get; init; }
    } 
}
