using Microservice.Shared;

namespace Microservice.Yonetici.Application.Features.YoneticiTipiFeatures.CreateYoneticiTipi
{
    public record CreateYoneticiTipiCommand(string TipAdi, int Value) : IRequestByServiceResult;
}
