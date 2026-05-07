using Microservice.Shared;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.HaberDtos;

namespace Mikroservice.Site.Application.Features.HaberFeatures.GetPaginatedHaber
{
    public record GetPaginatedHaberQuery(
    int SiteId, 
    int DilId,
    int Page = 1,
    int PageSize = 10,
    string? Search = null,
    string? OrderBy = "Id",
    string? OrderDir = "desc"
) : IRequestByServiceResult<PaginatedResult<HaberDto>>;
}
