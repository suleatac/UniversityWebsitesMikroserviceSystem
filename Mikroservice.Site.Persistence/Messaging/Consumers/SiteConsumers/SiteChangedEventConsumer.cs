using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SiteEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.SiteConsumers
{
    public class SiteChangedEventConsumer : IConsumer<SiteChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public SiteChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<SiteChangedEvent> context)
        {
            var message = context.Message;

            var key = $"site:list";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
