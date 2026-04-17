using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.PersonelTipEvents;
using Microservice.Shared.Services.RedisServiceItems;

namespace Mikroservice.Site.Persistence.Messaging.Consumers.PersonelTipConsumers
{
    public class PersonelTipChangedEventConsumer : IConsumer<PersonelTipChangedEvent>
    {
        private readonly IRedisCacheService _cache;

        public PersonelTipChangedEventConsumer(IRedisCacheService cache)
        {
            _cache = cache;
        }

        public async Task Consume(ConsumeContext<PersonelTipChangedEvent> context)
        {
            var message = context.Message;

            var key = $"personelTip:list";


            await _cache.RemoveAsync(key, context.CancellationToken);
        }
    }
}
