using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.PageTypeDtos;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypes
{
    public record GetPageTypesQuery : IRequestByServiceResult<List<PageTypeDto>>;
}