using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SiteOzellikleriEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.SiteOzellikleriConsumers
{
    public class SiteOzellikleriChangedEventConsumer : IConsumer<SiteOzellikleriChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public SiteOzellikleriChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<SiteOzellikleriChangedEvent> context)
        {
            var message = context.Message;

            var key = $"siteozellikleri:list:{message.SiteId}";


            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
