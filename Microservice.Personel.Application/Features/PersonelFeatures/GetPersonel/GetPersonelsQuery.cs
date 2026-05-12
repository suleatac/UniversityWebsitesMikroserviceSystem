using Microservice.Personel.Application.Contracts.DTOs;
using Microservice.Shared;

namespace Microservice.Personel.Application.Features.PersonelFeatures.GetPersonel
{
    public record GetPersonelsQuery : IRequestByServiceResult<List<PersonelDto>>;
}
