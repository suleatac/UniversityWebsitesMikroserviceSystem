using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.DeleteDuyuru
{
    public record DeleteDuyuruCommand(int Id) : IRequestByServiceResult;
}
