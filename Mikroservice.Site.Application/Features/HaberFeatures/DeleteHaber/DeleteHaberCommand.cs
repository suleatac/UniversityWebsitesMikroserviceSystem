using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.HaberFeatures.DeleteHaber
{
    public record DeleteHaberCommand(int Id) : IRequestByServiceResult;
}
