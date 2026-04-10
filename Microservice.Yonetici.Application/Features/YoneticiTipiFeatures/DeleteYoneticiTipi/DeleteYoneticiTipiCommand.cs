using Microservice.Shared;

namespace Microservice.Yonetici.Application.Features.YoneticiTipiFeatures.DeleteYoneticiTipi
{
    public record DeleteYoneticiTipiCommand(int Id) : IRequestByServiceResult;
}
