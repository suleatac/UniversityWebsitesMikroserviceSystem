using MediatR;

namespace Microservice.Ogrenci.Application.Features.OgrenciFeatures.GetOgrenci
{
    public record GetOgrencisQuery : IRequest<List<Domain.Entities.Ogrenci>>;
}
