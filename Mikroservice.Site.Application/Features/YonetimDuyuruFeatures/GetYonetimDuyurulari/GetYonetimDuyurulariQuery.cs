using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.YonetimDuyuru;

namespace Microservice.Site.Application.Features.YonetimDuyuruFeatures.GetYonetimDuyurulari
{
    public class GetYonetimDuyurulariQuery : IRequestByServiceResult<List<YonetimDuyuruDto>>;
}
