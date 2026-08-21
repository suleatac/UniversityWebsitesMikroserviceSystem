using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SiteEvents;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Persistence.Services;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.SiteConsumers
{
    public class SiteChangedEventConsumer(
        SiteNginxConfigService siteNginxConfigService,
        ILogger<SiteChangedEventConsumer> logger) : IConsumer<SiteChangedEvent>
    {
        public async Task Consume(ConsumeContext<SiteChangedEvent> context)
        {
            var message = context.Message;

            logger.LogInformation(
                "SiteChangedEvent alındı. SiteId: {SiteId}, SiteAlanAdi: {SiteAlanAdi}, PreviousSiteAlanAdi: {PreviousSiteAlanAdi}, ChangeType: {ChangeType}",
                message.SiteId,
                message.SiteAlanAdi,
                message.PreviousSiteAlanAdi,
                message.ChangeType);

            await siteNginxConfigService.ApplyAsync(
                message.SiteAlanAdi,
                message.PreviousSiteAlanAdi,
                message.ChangeType == SiteChangeType.Deleted,
                context.CancellationToken);
        }
    }
}