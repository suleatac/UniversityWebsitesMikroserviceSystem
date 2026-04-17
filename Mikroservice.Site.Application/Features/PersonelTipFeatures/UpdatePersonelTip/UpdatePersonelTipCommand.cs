using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.PersonelTipFeatures.UpdatePersonelTip
{
    public record UpdatePersonelTipCommand : IRequestByServiceResult
    {
        public int Id { get; init; }
        public string Ad { get; init; } = default!;
    }
}
