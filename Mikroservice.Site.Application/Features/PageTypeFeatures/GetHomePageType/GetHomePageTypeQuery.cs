using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.PageTypeDtos;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.GetHomePageType
{
    public record GetHomePageTypeQuery(int SiteId, int DilId) : IRequestByServiceResult<PageTypeDto>;
}