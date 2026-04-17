using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.CreatePersonelTip
{
    public record CreatePersonelTipCommand : IRequestByServiceResult
    {
        public string Ad { get; init; } = default!;
    }
}
