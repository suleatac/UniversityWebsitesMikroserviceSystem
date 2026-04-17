using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.DuyuruEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.DuyuruConsumers
{
    public class DuyuruCreatedEventConsumer : IConsumer<DuyuruCreatedEvent>
    {
        private readonly IRedisCacheService _cache;

        public DuyuruCreatedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<DuyuruCreatedEvent> context)
        {
            var message = context.Message;

            var key = $"duyurus:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
