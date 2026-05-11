using Microservice.Shared;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.PopupDtos;

namespace Mikroservice.Site.Application.Features.PopupFeatures.GetPaginatedPopup
{
    public record GetPaginatedPopupQuery(
        int SiteId,
        int DilId,
        int Page = 1,
        int PageSize = 10,
        string? Search = null,
        string? OrderBy = "Id",
        string? OrderDir = "desc"
    ) : IRequestByServiceResult<PaginatedResult<PopupDto>>;
}