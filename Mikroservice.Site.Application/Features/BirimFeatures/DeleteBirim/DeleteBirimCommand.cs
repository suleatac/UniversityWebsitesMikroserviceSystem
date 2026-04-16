using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.BirimFeatures.DeleteBirim
{
    public record DeleteBirimCommand(int Id) : IRequestByServiceResult;
}
