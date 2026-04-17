using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.PopupEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.PopupConsumers
{
    public class PopupChangedEventConsumer : IConsumer<PopupChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public PopupChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<PopupChangedEvent> context)
        {
            var message = context.Message;

            var key = $"popup:list:{message.SiteId}:{message.DilId}";


            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}