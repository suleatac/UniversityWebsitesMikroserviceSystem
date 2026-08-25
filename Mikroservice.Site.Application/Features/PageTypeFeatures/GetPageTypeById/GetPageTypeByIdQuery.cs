using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.PageTypeDtos;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypeById
{
    public record GetPageTypeByIdQuery(int Id) : IRequestByServiceResult<PageTypeDto>;
}