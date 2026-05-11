using Microservice.Shared;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.BannerDtos;

namespace Mikroservice.Site.Application.Features.BannerFeatures.GetPaginatedBanner
{
    public record GetPaginatedBannerQuery(
        int SiteId,
        int DilId,
        int Page = 1,
        int PageSize = 10,
        string? Search = null,
        string? OrderBy = "Id",
        string? OrderDir = "desc"
    ) : IRequestByServiceResult<PaginatedResult<BannerDto>>;
}