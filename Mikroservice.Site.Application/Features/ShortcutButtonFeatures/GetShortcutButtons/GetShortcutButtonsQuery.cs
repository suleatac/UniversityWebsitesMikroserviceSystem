using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.ShortcutButtonDtos;

namespace Mikroservice.Site.Application.Features.ShortcutButtonFeatures.GetShortcutButtons
{
 
    public record GetShortcutButtonsQuery(int SiteId, int DilId) : IRequestByServiceResult<List<ShortcutButtonDto>>;
}
