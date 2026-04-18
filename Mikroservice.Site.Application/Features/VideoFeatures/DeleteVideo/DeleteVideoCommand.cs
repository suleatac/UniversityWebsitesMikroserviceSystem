using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.VideoFeatures.DeleteVideo
{
    public record DeleteVideoCommand(int Id) : IRequestByServiceResult;
}
