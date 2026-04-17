using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.GetPersonelTip
{
    public record GetPersonelTipQuery : IRequestByServiceResult<List<PersonelTip>>;
}
