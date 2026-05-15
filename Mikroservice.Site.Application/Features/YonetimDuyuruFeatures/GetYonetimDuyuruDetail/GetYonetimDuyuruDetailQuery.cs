using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.YonetimDuyuru;

namespace Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.GetYonetimDuyuruDetail
{
    public record GetYonetimDuyuruDetailQuery(int Id, string KeycloakUserId) : IRequestByServiceResult<YonetimDuyuruDetailDto>;
}
