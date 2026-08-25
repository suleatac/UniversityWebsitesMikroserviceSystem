using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.PageTypeDtos;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypeBySlug
{
    public record GetPageTypeBySlugQuery(int SiteId, int DilId, string Slug) : IRequestByServiceResult<PageTypeDto>;
}