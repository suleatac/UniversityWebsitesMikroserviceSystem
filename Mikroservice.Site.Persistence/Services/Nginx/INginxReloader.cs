namespace Mikroservice.Site.Persistence.Services.Nginx
{
    /// <summary>
    /// nginx'in yeni yapılandırmayı yüklemesini sağlar.
    /// </summary>
    public interface INginxReloader
    {
        /// <summary>
        /// Reload isteği kaydeder. İstekler birleştirilir (debounce); art arda gelen
        /// site oluşturma işlemleri tek bir reload ile sonuçlanır.
        /// </summary>
        Task RequestReloadAsync(CancellationToken cancellationToken = default);
    }
}
