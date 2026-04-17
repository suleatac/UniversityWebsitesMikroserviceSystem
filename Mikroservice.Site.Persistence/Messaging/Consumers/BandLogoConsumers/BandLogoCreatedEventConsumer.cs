using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.BandLogoEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.BandLogoConsumers
{
    public class BandLogoCreatedEventConsumer : IConsumer<BandLogoCreatedEvent>
    {
        private readonly IRedisCacheService _cache;

        public BandLogoCreatedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<BandLogoCreatedEvent> context)
        {
            var message = context.Message;

            var key = $"bandlogos:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
