using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.GetUnvans
{
    public record GetUnvansQuery : IRequestByServiceResult<List<Unvan>>;

}
