using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.PageTypeDtos;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.GetHomePageType
{
    public record GetHomePageTypeQuery(int SiteTemplateId, int DilId) : IRequestByServiceResult<PageTypeDto>;
}