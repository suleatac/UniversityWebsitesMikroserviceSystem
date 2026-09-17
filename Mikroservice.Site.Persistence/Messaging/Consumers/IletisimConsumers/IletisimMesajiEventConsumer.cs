using MailKit.Net.Smtp;
using MailKit.Security;
using MassTransit;
using Microservice.Site.Persistence;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.IletisimEvents;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.IletisimConsumers
{
    /// <summary>
    /// Iletisim formundan gelen mesaji, sitenin kendi SMTP bilgileriyle
    /// yine sitenin kendi e-posta adresine ulastirir.
    /// SMTP kimlik bilgileri kuyrukta tasinmadigi icin site DB'den okunur.
    /// </summary>
    public class IletisimMesajiEventConsumer : IConsumer<IletisimMesajiEvent>
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<IletisimMesajiEventConsumer> _logger;

        public IletisimMesajiEventConsumer(
            AppDbContext dbContext,
            ILogger<IletisimMesajiEventConsumer> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IletisimMesajiEvent> context)
        {
            var message = context.Message;

            var site = _dbContext.Siteler
                .Where(x => x.Id == message.SiteId && !x.IsDeleted)
                .Select(x => new {
                    x.SiteAdi,
                    x.SiteEPosta,
                    x.SiteEPostaHost,
                    x.SiteEPostaPort,
                    x.SiteEPostaSifre
                })
                .FirstOrDefault();

            if (site is null)
            {
                _logger.LogWarning(
                    "Iletisim mesaji tuketilemedi, site bulunamadi. SiteId: {SiteId}",
                    message.SiteId);
                return;
            }

            var mail = new MimeMessage();
            mail.From.Add(new MailboxAddress(message.SiteAdi, site.SiteEPosta));
            mail.To.Add(new MailboxAddress(site.SiteAdi, site.SiteEPosta));
            mail.ReplyTo.Add(new MailboxAddress(message.AdSoyad, message.Eposta));
            mail.Subject = $"[Iletisim] {message.Konu}";

            mail.Body = new TextPart("plain") {
                Text = $"""
                    Yeni bir iletisim formu mesaji alindi.

                    Ad Soyad : {message.AdSoyad}
                    E-posta  : {message.Eposta}
                    Telefon  : {message.Telefon ?? "-"}
                    Konu     : {message.Konu}
                    Tarih    : {message.GonderimZamani:dd.MM.yyyy HH:mm}

                    Mesaj:
                    -------
                    {message.Mesaj}
                    """
            };

            using var smtp = new SmtpClient();

            // Port 465 => SMTPS, digerleri => STARTTLS (yonetim panosundaki kayitlara gore).
            var socketOptions = site.SiteEPostaPort == 465
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTlsWhenAvailable;

            await smtp.ConnectAsync(site.SiteEPostaHost, site.SiteEPostaPort, socketOptions, context.CancellationToken);
            await smtp.AuthenticateAsync(site.SiteEPosta, site.SiteEPostaSifre, context.CancellationToken);
            await smtp.SendAsync(mail, context.CancellationToken);
            await smtp.DisconnectAsync(true, context.CancellationToken);

            _logger.LogInformation(
                "Iletisim mesaji mail olarak gonderildi. SiteId: {SiteId}, Gonderen: {Eposta}",
                message.SiteId,
                message.Eposta);
        }
    }
}
