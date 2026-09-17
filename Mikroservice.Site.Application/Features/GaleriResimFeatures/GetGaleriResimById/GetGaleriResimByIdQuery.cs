using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.GaleriResimDtos;

namespace Mikroservice.Site.Application.Features.GaleriResimFeatures.GetGaleriResimById
{
    public record GetGaleriResimByIdQuery(int Id) : IRequestByServiceResult<GaleriResimDetailDto>;
}
