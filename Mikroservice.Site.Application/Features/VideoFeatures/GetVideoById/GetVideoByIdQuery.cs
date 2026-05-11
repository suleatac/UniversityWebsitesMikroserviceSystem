using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.VideoDtos;

namespace Mikroservice.Site.Application.Features.VideoFeatures.GetVideoById
{
    public record GetVideoByIdQuery(int Id) : IRequestByServiceResult<VideoDetailDto>;
}