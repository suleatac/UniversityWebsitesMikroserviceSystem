using Microservice.Shared;

namespace Microservice.Site.Application.Features.YoneticiDuyuruFeatures.DeleteYoneticiDuyuru
{
    public record DeleteYoneticiDuyuruCommand(int Id) : IRequestByServiceResult;
}
