using Microservice.Shared;

namespace Microservice.Site.Application.Features.YonetimDuyuruFeatures.DeleteYonetimDuyuru
{
    public record DeleteYonetimDuyuruCommand(int Id) : IRequestByServiceResult;
}
