using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.DeletePersonelTip
{
    public record DeletePersonelTipCommand(int Id) : IRequestByServiceResult;
}
