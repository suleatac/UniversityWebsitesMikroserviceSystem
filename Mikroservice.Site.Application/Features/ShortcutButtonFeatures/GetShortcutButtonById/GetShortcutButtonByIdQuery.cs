using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.ShortcutButtonDtos;

namespace Mikroservice.Site.Application.Features.ShortcutButtonFeatures.GetShortcutButtonById
{

    public record GetShortcutButtonByIdQuery(int Id) : IRequestByServiceResult<ShortcutButtonDetailDto>;
}
