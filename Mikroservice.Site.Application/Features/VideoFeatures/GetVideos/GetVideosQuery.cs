using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.VideoDtos;

namespace Mikroservice.Site.Application.Features.VideoFeatures.GetVideos
{
    public record GetVideosQuery(int SiteId, int DilId) : IRequestByServiceResult<List<VideoDto>>;
}
