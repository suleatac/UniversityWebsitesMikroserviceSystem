using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.BannerDtos;

namespace Mikroservice.Site.Application.Features.BannerFeatures.GetBannerById
{
    public record GetBannerByIdQuery(int Id) : IRequestByServiceResult<BannerDetailDto>;
}