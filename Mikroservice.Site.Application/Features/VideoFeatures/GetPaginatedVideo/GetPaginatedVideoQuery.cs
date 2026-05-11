using Microservice.Shared;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.VideoDtos;

namespace Mikroservice.Site.Application.Features.VideoFeatures.GetPaginatedVideo
{
    public record GetPaginatedVideoQuery(
        int SiteId,
        int DilId,
        int Page = 1,
        int PageSize = 10,
        string? Search = null,
        string? OrderBy = "Id",
        string? OrderDir = "desc"
    ) : IRequestByServiceResult<PaginatedResult<VideoDto>>;
}