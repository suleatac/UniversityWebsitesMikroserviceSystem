using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.MenuEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.MenuConsumers
{
    public class MenuChangedEventConsumer : IConsumer<MenuChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public MenuChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<MenuChangedEvent> context)
        {
            var message = context.Message;

            var key = $"menus:list:{message.SiteId}:{message.DilId}";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
