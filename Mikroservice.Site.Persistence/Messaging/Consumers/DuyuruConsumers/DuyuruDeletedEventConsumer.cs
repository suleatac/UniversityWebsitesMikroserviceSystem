using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.DuyuruEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.DuyuruConsumers
{
    public class DuyuruDeletedEventConsumer : IConsumer<DuyuruDeletedEvent>
    {
        private readonly IRedisCacheService _cache;

        public DuyuruDeletedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<DuyuruDeletedEvent> context)
        {
            var message = context.Message;

            var key = $"duyurus:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
