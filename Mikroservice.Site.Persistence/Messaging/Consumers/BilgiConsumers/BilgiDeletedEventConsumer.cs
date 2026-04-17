using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BilgiEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.BilgiConsumers
{
    public class BilgiDeletedEventConsumer : IConsumer<BilgiDeletedEvent>
    {
        private readonly IRedisCacheService _cache;

        public BilgiDeletedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<BilgiDeletedEvent> context)
        {
            var message = context.Message;

            var key = $"bilgis:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
