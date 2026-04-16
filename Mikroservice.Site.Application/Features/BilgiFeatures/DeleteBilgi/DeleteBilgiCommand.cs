using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.DeleteBilgi
{
    public record DeleteBilgiCommand(int Id) : IRequestByServiceResult;
}
