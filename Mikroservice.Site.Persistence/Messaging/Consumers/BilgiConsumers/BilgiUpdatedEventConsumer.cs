using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BilgiEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.BilgiConsumers
{
    public class BilgiUpdatedEventConsumer : IConsumer<BilgiUpdatedEvent>
    {
        private readonly IRedisCacheService _cache;

        public BilgiUpdatedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<BilgiUpdatedEvent> context)
        {
            var message = context.Message;

            var key = $"bilgis:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
