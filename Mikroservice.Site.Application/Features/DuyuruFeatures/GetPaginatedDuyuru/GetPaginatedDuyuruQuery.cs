using Microservice.Shared;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.DuyuruDtos;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.GetPaginatedDuyuru
{
    public record GetPaginatedDuyuruQuery(
        int SiteId,
        int DilId,
        int Page = 1,
        int PageSize = 10,
        string? Search = null,
        string? OrderBy = "Id",
        string? OrderDir = "desc"
    ) : IRequestByServiceResult<PaginatedResult<DuyuruDto>>;
}