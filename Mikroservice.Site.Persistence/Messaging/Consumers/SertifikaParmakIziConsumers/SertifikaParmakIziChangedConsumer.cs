using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SertifikaParmakIziEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.SertifikaParmakIziConsumers
{
    public class SertifikaParmakIziChangedConsumer : IConsumer<SertifikaParmakIziChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public SertifikaParmakIziChangedConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<SertifikaParmakIziChangedEvent> context)
        {
            var message = context.Message;

            var key = $"sertifikaparmakizi:list";

            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
