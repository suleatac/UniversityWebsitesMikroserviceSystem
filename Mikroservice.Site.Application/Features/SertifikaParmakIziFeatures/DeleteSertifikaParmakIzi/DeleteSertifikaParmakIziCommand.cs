using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.DeleteSertifikaParmakIzi
{
    public record DeleteSertifikaParmakIziCommand(int Id) : IRequestByServiceResult;
}
