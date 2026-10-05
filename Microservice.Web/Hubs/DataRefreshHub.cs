using Microsoft.AspNetCore.SignalR;

namespace Microservice.Web.Hubs
{
    /// <summary>
    /// Tip-guvenli SignalR sozlesmesi: sunucunun istemcilere gonderebilecegi
    /// olaylari tanimlar. IHubContext<DataRefreshHub, IDataRefresh> ile
    /// string degil lambda ile gonderim yapilir; method ad imzasi derleme zamaninda dogrulanir.
    /// </summary>
    public interface IDataRefreshHub
    {
        /// <summary>
        /// Hangfire senkronizasyonu tamamlaninca acik sayfalara yayinlanan sinyal.
        /// data-refresh.js bu olayi "DataChanged" adiyla dinler.
        /// </summary>
        Task DataChanged(DataChangedMessage message);
    }

    /// <summary>
    /// Veri degisikligi bildirim hub'i. Hangfire senkronizasyonlari tamamlandiginda
    /// MassTransit consumer'i tarafindan "DataChanged" mesaji yayinlanir; acik olan
    /// template sayfalari bu mesaji alip iceriklerini tazeler.
    /// </summary>
    public class DataRefreshHub : Hub<IDataRefreshHub>
    {
        //// Ileride site bazli daraltma icin grup mekanizmasi hazir:
        //// su an event'ler global oldugu icin Clients.All kullaniliyor.
        //public Task JoinSiteGroup(int siteId)
        //{
        //    return Groups.AddToGroupAsync(Context.ConnectionId, $"site-{siteId}");
        //}

        //public Task LeaveSiteGroup(int siteId)
        //{
        //    return Groups.RemoveFromGroupAsync(Context.ConnectionId, $"site-{siteId}");
        //}
    }
}
