using Microservice.Shared;

namespace Microservice.Yonetici.Application.Features.YoneticiDuyuruFeatures.DeleteYoneticiDuyuru
{
    public record DeleteYoneticiDuyuruCommand(int Id) : IRequestByServiceResult;
}
