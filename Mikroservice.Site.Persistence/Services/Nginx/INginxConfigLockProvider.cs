namespace Mikroservice.Site.Persistence.Services.Nginx
{
    /// <summary>
    /// nginx yapılandırmasına yazma işlemlerini serileştiren dağıtık kilit sağlayıcısı.
    /// </summary>
    public interface INginxConfigLockProvider
    {
        /// <summary>
        /// Belirtilen kaynak için kilit alır. Kilit alınamazsa null döner (işlem atlanmalıdır).
        /// Dönen handle dispose edildiğinde kilit serbest bırakılır.
        /// </summary>
        Task<IAsyncDisposable?> AcquireAsync(
            string resource,
            TimeSpan leaseTime,
            TimeSpan waitTime,
            CancellationToken cancellationToken = default);
    }
}
