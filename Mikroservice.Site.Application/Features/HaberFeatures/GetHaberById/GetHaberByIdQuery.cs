using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.HaberDtos;

namespace Mikroservice.Site.Application.Features.HaberFeatures.GetHaberById
{
    public record GetHaberByIdQuery(int Id) : IRequestByServiceResult<HaberDetailDto>;

}
