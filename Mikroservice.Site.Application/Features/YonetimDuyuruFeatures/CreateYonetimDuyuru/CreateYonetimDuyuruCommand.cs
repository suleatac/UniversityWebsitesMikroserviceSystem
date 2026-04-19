using Microservice.Shared;

namespace Microservice.Site.Application.Features.YonetimDuyuruFeatures.CreateYonetimDuyuru
{
    public record CreateYonetimDuyuruCommand(string Baslik, string Icerik, DateTime EklenmeTarihi, bool Aktif) : IRequestByServiceResult;
}
