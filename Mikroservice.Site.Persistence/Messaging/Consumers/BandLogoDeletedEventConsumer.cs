using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers
{
    public class BandLogoDeletedEventConsumer : IConsumer<BandLogoDeletedEvent>
    {
        private readonly IRedisCacheService _cache;

        public BandLogoDeletedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<BandLogoDeletedEvent> context)
        {
            var message = context.Message;

            var key = $"bandlogos:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
