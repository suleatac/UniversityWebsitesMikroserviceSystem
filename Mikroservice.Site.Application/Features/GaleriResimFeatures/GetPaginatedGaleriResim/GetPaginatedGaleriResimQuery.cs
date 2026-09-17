using Microservice.Shared;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.GaleriResimDtos;

namespace Mikroservice.Site.Application.Features.GaleriResimFeatures.GetPaginatedGaleriResim
{
    public record GetPaginatedGaleriResimQuery(
        int SiteId,
        int DilId,
        int Page = 1,
        int PageSize = 10,
        string? Search = null,
        string? Kategori = null,
        string? OrderBy = "Id",
        string? OrderDir = "desc"
    ) : IRequestByServiceResult<PaginatedResult<GaleriResimDto>>;
}
