using Microservice.Shared;

namespace Microservice.Yonetici.Application.Features.YoneticiDuyuruFeatures.CreateYoneticiDuyuru
{
    public record CreateYoneticiDuyuruCommand(string Baslik, string Icerik, DateTime EklenmeTarihi, bool Aktif) : IRequestByServiceResult;
}
