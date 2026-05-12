using Microservice.Shared;

namespace Microservice.Personel.Application.Features.PersonelFeatures.GetPersonelById
{
    public record GetPersonelByIdQuery(int Id) : IRequestByServiceResult<Domain.Entities.Personel>;
}
