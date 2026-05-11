using Microservice.Shared;
using Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.CreateYonetimDuyuru;

namespace Microservice.Site.Application.Features.YonetimDuyuruFeatures.CreateYonetimDuyuru
{
    public record CreateYonetimDuyuruCommand(
        string Baslik,
        string Icerik) : IRequestByServiceResult<CreateYonetimDuyuruResponse>;
}
