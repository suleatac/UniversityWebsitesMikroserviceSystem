using Microservice.Shared;

namespace Microservice.Yonetici.Application.Features.YoneticiTipiFeatures.UpdateYoneticiTipi
{
    public record UpdateYoneticiTipiCommand(int Id, string TipAdi, int Value) : IRequestByServiceResult;
}
