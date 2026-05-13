using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.PopupFeatures.GetPopup
{
    public record GetPopupQuery(int SiteId) : IRequestByServiceResult<Popup>;
}
