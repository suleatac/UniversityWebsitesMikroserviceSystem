using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.MenuFeatures.DeleteMenu
{
    public record DeleteMenuCommand(int Id) : IRequestByServiceResult;
}
