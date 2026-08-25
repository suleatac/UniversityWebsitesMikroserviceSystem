using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.DuyuruDtos;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.GetDuyuruBySeoUrl
{
    public record GetDuyuruBySeoUrlQuery(int SiteId, int DilId, string SeoUrl) : IRequestByServiceResult<DuyuruDetailDto>;
}