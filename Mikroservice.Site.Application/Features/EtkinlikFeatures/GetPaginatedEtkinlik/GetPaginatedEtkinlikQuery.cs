using Microservice.Shared;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.EtkinlikDtos;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.GetPaginatedEtkinlik
{
    public record GetPaginatedEtkinlikQuery(
        int SiteId,
        int DilId,
        int Page = 1,
        int PageSize = 10,
        string? Search = null,
        string? OrderBy = "Id",
        string? OrderDir = "desc"
    ) : IRequestByServiceResult<PaginatedResult<EtkinlikDto>>;
}