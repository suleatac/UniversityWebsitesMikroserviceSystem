using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.ShortcutButtonFeatures.DeleteShortcutButton
{

    public record DeleteShortcutButtonCommand(int Id) : IRequestByServiceResult;
}
