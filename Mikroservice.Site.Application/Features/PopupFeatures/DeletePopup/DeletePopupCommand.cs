using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.PopupFeatures.DeletePopup
{
    public record DeletePopupCommand(int Id) : IRequestByServiceResult;
}
