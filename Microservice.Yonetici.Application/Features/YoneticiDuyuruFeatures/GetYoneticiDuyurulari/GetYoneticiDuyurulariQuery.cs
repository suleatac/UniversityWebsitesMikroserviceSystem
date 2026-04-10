using Microservice.Shared;
using Microservice.Yonetici.Domain.Entities;

namespace Microservice.Yonetici.Application.Features.YoneticiDuyuruFeatures.GetYoneticiDuyurulari
{
    public class GetYoneticiDuyurulariQuery : IRequestByServiceResult<List<YoneticiDuyuru>>;
}
