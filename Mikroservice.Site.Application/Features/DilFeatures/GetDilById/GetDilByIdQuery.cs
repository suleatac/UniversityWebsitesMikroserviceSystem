using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.DilFeatures.GetDilById
{
    public record GetDilByIdQuery(int Id) : IRequestByServiceResult<Dil>;
}
