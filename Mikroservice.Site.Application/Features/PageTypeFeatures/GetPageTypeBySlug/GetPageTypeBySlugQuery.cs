using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.PageTypeDtos;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypeBySlug
{
    public record GetPageTypeBySlugQuery(int TemplateId, int DilId, string Slug) : IRequestByServiceResult<PageTypeDto>;
}