using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.DeleteUnvan
{
    public record DeleteUnvanCommand(int Id) : IRequestByServiceResult;

}
