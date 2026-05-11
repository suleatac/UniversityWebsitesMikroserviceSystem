using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.PopupDtos;

namespace Mikroservice.Site.Application.Features.PopupFeatures.GetPopupById
{
    public record GetPopupByIdQuery(int Id) : IRequestByServiceResult<PopupDetailDto>;
}