using Microservice.Shared;

namespace Microservice.Yonetici.Application.Features.YoneticiDuyuruFeatures.UpdateYoneticiDuyuru
{
    public record UpdateYoneticiDuyuruCommand(int Id, string Baslik, string Icerik, DateTime EklenmeTarihi, bool Aktif) : IRequestByServiceResult;
}
