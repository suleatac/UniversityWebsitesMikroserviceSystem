using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.CreateSertifikaParmakIzi
{
    public record CreateSertifikaParmakIziCommand : IRequestByServiceResult
    {
        public string SertifikaParmakIziNumarasi { get; init; } = default!;
        public DateTime? SertifikaYili { get; init; }
        public bool Aktif { get; init; }
    }
}
