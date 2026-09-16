using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.VideoDtos;

namespace Mikroservice.Site.Application.Features.VideoFeatures.GetVideoBySeoUrl
{
    public record GetVideoBySeoUrlQuery(int SiteId, int DilId, string SeoUrl) : IRequestByServiceResult<VideoDetailDto>;
}
