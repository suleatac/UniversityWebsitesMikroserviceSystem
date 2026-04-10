using Microservice.Shared;
using Microservice.Yonetici.Domain.Entities;

namespace Microservice.Yonetici.Application.Features.YoneticiTipiFeatures.GetYoneticiTipleri
{
    public class GetYoneticiTipleriQuery: IRequestByServiceResult<List<YoneticiTipi>>;
    
}
