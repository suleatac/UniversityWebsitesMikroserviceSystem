using Microservice.Shared;
using Microservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Features.YoneticiDuyuruFeatures.GetYoneticiDuyurulari
{
    public class GetYoneticiDuyurulariQuery : IRequestByServiceResult<List<YonetimDuyuru>>;
}
