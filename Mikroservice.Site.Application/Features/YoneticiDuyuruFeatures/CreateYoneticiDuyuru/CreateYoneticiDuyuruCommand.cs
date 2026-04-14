using Microservice.Shared;

namespace Microservice.Site.Application.Features.YoneticiDuyuruFeatures.CreateYoneticiDuyuru
{
    public record CreateYoneticiDuyuruCommand(string Baslik, string Icerik, DateTime EklenmeTarihi, bool Aktif) : IRequestByServiceResult;
}
