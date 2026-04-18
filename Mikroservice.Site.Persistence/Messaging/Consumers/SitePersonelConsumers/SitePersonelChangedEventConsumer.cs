using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SitePersonelEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.SitePersonelConsumers
{
    public class SitePersonelChangedEventConsumer : IConsumer<SitePersonelChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public SitePersonelChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<SitePersonelChangedEvent> context)
        {
            var message = context.Message;

            var key = $"sitepersonel:list:{message.SiteId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
