using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.DuyuruDtos;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.GetDuyurus
{
    public record GetDuyurusQuery(int SiteId, int DilId) : IRequestByServiceResult<List<DuyuruDto>>;
}
