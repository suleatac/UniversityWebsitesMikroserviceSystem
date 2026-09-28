using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.PageSectionDtos;

namespace Mikroservice.Site.Application.Features.PageSectionFeatures.GetPageSectionById
{
    public record GetPageSectionByIdQuery(int Id) : IRequestByServiceResult<PageSectionDto>;
}
