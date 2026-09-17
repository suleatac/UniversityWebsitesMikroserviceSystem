using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.GaleriResimFeatures.DeleteGaleriResim
{
    public record DeleteGaleriResimCommand(int Id) : IRequestByServiceResult;
}
