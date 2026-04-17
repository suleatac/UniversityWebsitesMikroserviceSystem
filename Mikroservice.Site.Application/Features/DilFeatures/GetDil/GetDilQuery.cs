using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.DilFeatures.GetDil
{
    public record GetDilQuery : IRequestByServiceResult<List<Dil>>;
}
