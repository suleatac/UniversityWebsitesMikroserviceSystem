using Microservice.Shared;

namespace Microservice.Site.Application.Features.YoneticiTipiFeatures.DeleteYoneticiTipi
{
    public record DeleteYoneticiTipiCommand(int Id) : IRequestByServiceResult;
}
