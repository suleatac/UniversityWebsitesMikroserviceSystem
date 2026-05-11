using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.GetPersonelTipById
{
    public record GetPersonelTipByIdQuery(int Id) : IRequestByServiceResult<PersonelTip>;
}