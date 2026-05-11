using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.EtkinlikDtos;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.GetEtkinlikById
{
    public record GetEtkinlikByIdQuery(int Id) : IRequestByServiceResult<EtkinlikDetailDto>;
}