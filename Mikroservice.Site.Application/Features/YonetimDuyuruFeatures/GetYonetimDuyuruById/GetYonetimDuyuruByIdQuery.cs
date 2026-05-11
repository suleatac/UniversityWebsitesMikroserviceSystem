using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.YonetimDuyuru;

namespace Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.GetYonetimDuyuruById
{
    public record GetYonetimDuyuruByIdQuery(int Id) : IRequestByServiceResult<YonetimDuyuruDto>;
}
