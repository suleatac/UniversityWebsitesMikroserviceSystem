using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.SitePersonelDtos;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.GetSitePersonelBySeoUrl
{
    public record GetSitePersonelBySeoUrlQuery(int SiteId, string SeoUrl) : IRequestByServiceResult<SitePersonelDetailDto>;
}
