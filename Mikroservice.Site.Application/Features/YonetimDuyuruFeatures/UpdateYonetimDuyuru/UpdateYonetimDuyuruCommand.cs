using Microservice.Shared;

namespace Microservice.Site.Application.Features.YonetimDuyuruFeatures.UpdateYonetimDuyuru
{
    public record UpdateYonetimDuyuruCommand(int Id, string Baslik, string Icerik, DateTime EklenmeTarihi, bool Aktif) : IRequestByServiceResult;
}
