using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.UpdateSertifikaParmakIzi
{
    public record UpdateSertifikaParmakIziCommand : IRequestByServiceResult
    {
        public int Id { get; init; }
        public string SertifikaParmakIziNumarasi { get; init; } = default!;
        public DateTime? SertifikaYili { get; init; }
        public bool Aktif { get; init; }
    }
}
