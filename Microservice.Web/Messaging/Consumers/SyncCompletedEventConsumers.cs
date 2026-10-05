using MassTransit;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.SyncEvents;
using Microservice.Web.Hubs;
using Microservice.Web.Services;
using Microsoft.AspNetCore.SignalR;

namespace Microservice.Web.Messaging.Consumers
{
    /// <summary>
    /// Personel senkronizasyonu bittiginde: once Redis cache'ini duser (refetch taze veri alsin),
    /// sonra SignalR ile acik sayfalari bilgilendirir. Sira onemli: once cache, sonra yayin.
    /// Tip-guvenli gonderim: IHubContext<DataRefreshHub, IDataRefreshHub> + lambda;
    /// method adi ("DataChanged") derleme zamaninda dogrulanir, payload tipi sabittir.
    /// </summary>
    public class PersonelSyncCompletedEventConsumer(
        IRedisCacheService redisCacheService,
        IHubContext<DataRefreshHub, IDataRefreshHub> hubContext,
        ILogger<PersonelSyncCompletedEventConsumer> logger) : IConsumer<PersonelSyncCompletedEvent>
    {
        public async Task Consume(ConsumeContext<PersonelSyncCompletedEvent> context)
        {
            logger.LogInformation(
                "PersonelSyncCompletedEvent alindi. Islenen: {Count}", context.Message.ProcessedCount);

            await redisCacheService.RemoveByPatternAsync("personel:*");

            await hubContext.Clients.All.DataChanged(new DataChangedMessage(
                Scope: "personel",
                ProcessedCount: context.Message.ProcessedCount,
                CompletedAt: context.Message.CompletedAt));
        }
    }

    /// <summary>
    /// Ogrenci senkronizasyonu bittiginde: ogrenci cache anahtarlarini duser ve SignalR sinyali yayar.
    /// </summary>
    public class OgrenciSyncCompletedEventConsumer(
        IRedisCacheService redisCacheService,
        IHubContext<DataRefreshHub, IDataRefreshHub> hubContext,
        ILogger<OgrenciSyncCompletedEventConsumer> logger) : IConsumer<OgrenciSyncCompletedEvent>
    {
        public async Task Consume(ConsumeContext<OgrenciSyncCompletedEvent> context)
        {
            logger.LogInformation(
                "OgrenciSyncCompletedEvent alindi. Islenen: {Count}", context.Message.ProcessedCount);

            await redisCacheService.RemoveByPatternAsync("ogrenci:*");

            await hubContext.Clients.All.DataChanged(new DataChangedMessage(
                Scope: "ogrenci",
                ProcessedCount: context.Message.ProcessedCount,
                CompletedAt: context.Message.CompletedAt));
        }
    }
}
