using Microservice.Shared;
using Microservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Features.YonetimDuyuruFeatures.GetYonetimDuyurulari
{
    public class GetYonetimDuyurulariQuery : IRequestByServiceResult<List<YonetimDuyuru>>;
}
