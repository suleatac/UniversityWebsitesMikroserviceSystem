using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.IletisimEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;

namespace Mikroservice.Site.Application.Features.IletisimFeatures.GonderIletisimMesaji
{
    public class GonderIletisimMesajiCommandHandler(
        ISiteRepository siteRepository,
        IPublishEndpoint publishEndpoint,
        ILogger<GonderIletisimMesajiCommandHandler> logger
    ) : IRequestHandler<GonderIletisimMesajiCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(GonderIletisimMesajiCommand request, CancellationToken cancellationToken)
        {
            // Mail, sitenin kendi SMTP bilgileriyle yine sitenin kendi e-postasina gonderilecek;
            // bu yuzden once site dogrulanir.
            var site = await siteRepository.GetByIdAsync(request.SiteId);

            if (site is null || site.IsDeleted)
            {
                logger.LogWarning("Iletisim mesaji icin site bulunamadi. SiteId: {SiteId}", request.SiteId);
                return ServiceResult.Error("Site bulunamadı", System.Net.HttpStatusCode.NotFound);
            }

            if (string.IsNullOrWhiteSpace(site.SiteEPosta)
                || string.IsNullOrWhiteSpace(site.SiteEPostaHost)
                || site.SiteEPostaPort <= 0)
            {
                logger.LogError("Site iletisim maili icin SMTP bilgileri eksik. SiteId: {SiteId}", request.SiteId);
                return ServiceResult.Error("Site e-posta bilgileri eksik, lütfen daha sonra tekrar deneyin.", System.Net.HttpStatusCode.InternalServerError);
            }

            // SMTP kimlik bilgileri kuyrukta tasinmasın diye consumer DB'den siteyi tekrar okur;
            // event yalnizca icerik + site id tasir.
            await publishEndpoint.Publish(
                new IletisimMesajiEvent(
                    site.Id,
                    site.SiteAdi,
                    request.AdSoyad.Trim(),
                    request.Eposta.Trim(),
                    request.Konu.Trim(),
                    request.Mesaj.Trim(),
                    string.IsNullOrWhiteSpace(request.Telefon) ? null : request.Telefon.Trim(),
                    DateTime.Now),
                cancellationToken);

            logger.LogInformation(
                "Iletisim mesaji kuyruga basildi. SiteId: {SiteId}, Gonderen: {Eposta}",
                site.Id,
                request.Eposta);

            return ServiceResult.Success();
        }
    }
}
