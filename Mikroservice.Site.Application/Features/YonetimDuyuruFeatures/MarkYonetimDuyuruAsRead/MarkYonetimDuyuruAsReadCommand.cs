using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.MarkYonetimDuyuruAsRead
{
    public record MarkYonetimDuyuruAsReadCommand(int Id, string KeycloakUserId) : IRequestByServiceResult<bool>;
}
