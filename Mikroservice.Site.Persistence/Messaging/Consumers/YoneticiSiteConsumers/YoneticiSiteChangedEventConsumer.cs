using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.YoneticiSiteEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.YoneticiSiteConsumers
{
    public class YoneticiSiteChangedEventConsumer : IConsumer<YoneticiSiteChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public YoneticiSiteChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<YoneticiSiteChangedEvent> context)
        {
            var message = context.Message;

            var key = $"yoneticiSite:list:{message.SiteId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
