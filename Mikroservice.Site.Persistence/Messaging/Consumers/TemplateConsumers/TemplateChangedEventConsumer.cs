using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.TemplateEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.TemplateConsumers
{
    public class TemplateChangedEventConsumer : IConsumer<TemplateChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public TemplateChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<TemplateChangedEvent> context)
        {
            var message = context.Message;

            var key = "template:list";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
