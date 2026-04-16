using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.DeleteBandLogo
{
    public record DeleteBandLogoCommand(int Id) : IRequestByServiceResult;
}
