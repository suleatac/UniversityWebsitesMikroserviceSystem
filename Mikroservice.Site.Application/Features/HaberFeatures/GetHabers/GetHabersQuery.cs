using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.HaberDtos;

namespace Mikroservice.Site.Application.Features.HaberFeatures.GetHabers
{

    public record GetHabersQuery(int SiteId, int DilId) : IRequestByServiceResult<List<HaberDto>>;
}
