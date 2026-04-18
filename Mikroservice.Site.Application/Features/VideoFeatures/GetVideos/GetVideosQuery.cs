using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.VideoFeatures.GetVideos
{
    public record GetVideosQuery(int SiteId, int DilId) : IRequestByServiceResult<List<Video>>;
}
