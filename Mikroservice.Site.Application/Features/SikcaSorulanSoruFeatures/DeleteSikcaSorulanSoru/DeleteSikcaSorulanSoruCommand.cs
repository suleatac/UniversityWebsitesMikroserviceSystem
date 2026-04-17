using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.DeleteSikcaSorulanSoru
{
    public record DeleteSikcaSorulanSoruCommand(int Id) : IRequestByServiceResult;
}
