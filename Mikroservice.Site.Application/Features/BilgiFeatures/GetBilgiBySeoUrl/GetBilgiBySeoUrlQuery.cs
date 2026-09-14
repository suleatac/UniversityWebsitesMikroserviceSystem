using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.BilgiDtos;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.GetBilgiBySeoUrl
{
    public record GetBilgiBySeoUrlQuery(int SiteId, int DilId, string SeoUrl) : IRequestByServiceResult<BilgiDetailDto>;
}

