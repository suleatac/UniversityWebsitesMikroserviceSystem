using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.GetUnreadYonetimDuyuruCount
{
    public record GetUnreadYonetimDuyuruCountQuery(string KeycloakUserId) : IRequestByServiceResult<int>;
}
