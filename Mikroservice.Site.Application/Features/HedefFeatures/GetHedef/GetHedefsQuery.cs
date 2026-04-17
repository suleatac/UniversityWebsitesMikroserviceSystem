using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.HedefFeatures.GetHedef
{
    public record GetHedefsQuery : IRequestByServiceResult<List<Hedef>>;
}
