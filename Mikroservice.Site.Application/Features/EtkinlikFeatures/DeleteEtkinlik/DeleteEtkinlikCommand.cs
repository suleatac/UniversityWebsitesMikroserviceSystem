using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.DeleteEtkinlik
{
    public record DeleteEtkinlikCommand(int Id) : IRequestByServiceResult;
}
