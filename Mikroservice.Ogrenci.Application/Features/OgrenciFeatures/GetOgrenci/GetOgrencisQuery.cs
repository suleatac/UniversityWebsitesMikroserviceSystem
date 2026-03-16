using MediatR;
using Microservice.Shared;

namespace Microservice.Ogrenci.Application.Features.OgrenciFeatures.GetOgrenci
{
    public record GetOgrencisQuery : IRequestByServiceResult<List<Domain.Entities.Ogrenci>>;
}
