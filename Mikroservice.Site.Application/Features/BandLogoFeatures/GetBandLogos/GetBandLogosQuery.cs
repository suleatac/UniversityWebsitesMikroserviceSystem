using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.GetBandLogos
{
    public record GetBandLogosQuery(int SiteId, int DilId) : IRequestByServiceResult<List<BandLogo>>;
}
