using Microservice.Shared;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.BandLogoDtos;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.GetPaginatedBandLogo
{
    public record GetPaginatedBandLogoQuery(
        int SiteId,
        int DilId,
        int Page = 1,
        int PageSize = 10,
        string? Search = null,
        string? OrderBy = "Id",
        string? OrderDir = "desc"
    ) : IRequestByServiceResult<PaginatedResult<BandLogoDto>>;
}
