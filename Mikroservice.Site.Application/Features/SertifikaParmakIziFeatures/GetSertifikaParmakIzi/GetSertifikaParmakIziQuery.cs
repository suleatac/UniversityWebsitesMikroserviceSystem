using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.GetSertifikaParmakIzi
{
    public record GetSertifikaParmakIziQuery : IRequestByServiceResult<List<SertifikaParmakIzi>>;
}
