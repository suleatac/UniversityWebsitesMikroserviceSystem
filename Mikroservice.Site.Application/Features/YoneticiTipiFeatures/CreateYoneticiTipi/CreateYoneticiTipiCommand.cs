using Microservice.Shared;

namespace Microservice.Site.Application.Features.YoneticiTipiFeatures.CreateYoneticiTipi
{
    public record CreateYoneticiTipiCommand(string TipAdi, int Value) : IRequestByServiceResult;
}
