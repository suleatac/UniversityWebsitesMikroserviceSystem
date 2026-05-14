using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.SitePersonelDtos;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.GetSitePersonels
{
    public record GetSitePersonelQuery(int SiteId) : IRequestByServiceResult<List<SitePersonelDetailDto>>;

}
