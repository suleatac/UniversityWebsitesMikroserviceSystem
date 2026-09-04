using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.PageTypeDtos;
using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypeByTemplateIdAndPageTypeKind
{
    public record GetPageTypeByTemplateIdAndPageTypeKindQuery(
        int TemplateId,
        int DilId,
        PageTypeKind PageTypeKind) : IRequestByServiceResult<PageTypeDto>;
}
