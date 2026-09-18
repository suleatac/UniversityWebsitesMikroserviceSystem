using Microservice.Site.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mikroservice.Site.Persistence.Settings;

namespace Mikroservice.Site.Persistence.Services.Nginx
{
    /// <summary>
    /// nginx yapılandırmasını periyodik olarak veritabanıyla mutabık kılan arka plan çalışanı.
    /// <para>
    /// Neden gerekli: MassTransit kuyruğu kaybolabilir, uygulama olay işlenirken çökebilir veya
    /// conf.d dosyaları elle değiştirilebilir. Bu durumda nginx yapılandırması sessizce
    /// gerçek durumdan sapar (drift). Reconcile işi bu sapmayı kendiliğinden onarır ve
    /// <c>TemplateVersion</c> artırıldığında tüm dosyaların yeni şablona geçmesini sağlar.
    /// </para>
    /// </summary>
    public class NginxConfigReconcileHostedService
    (
        IServiceScopeFactory scopeFactory,
        IOptions<NginxConfigSetting> settings,
        ILogger<NginxConfigReconcileHostedService> logger
    ) : IHostedService, IDisposable
    {
        private CancellationTokenSource? _stopSource;
        private Task? _worker;
        private bool _disabled;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var configuration = settings.Value;

            if (!configuration.EnableReconcile)
            {
                logger.LogInformation("Nginx conf reconcile işi devre dışı (EnableReconcile=false).");
                _disabled = true;
                return Task.CompletedTask;
            }

            _stopSource = new CancellationTokenSource();
            _worker = Task.Run(() => RunAsync(_stopSource.Token), CancellationToken.None);

            logger.LogInformation(
                "Nginx conf reconcile işi başlatıldı. İlk çalışma: {Delay}s sonra, aralık: {Interval} dakika, şablon sürümü: {Version}.",
                configuration.ReconcileStartupDelaySeconds,
                configuration.ReconcileIntervalMinutes,
                configuration.TemplateVersion);

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_disabled || _worker is null || _stopSource is null)
            {
                return;
            }

            await _stopSource.CancelAsync();

            try
            {
                await _worker.WaitAsync(TimeSpan.FromSeconds(5), cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogDebug(ex, "Nginx conf reconcile işi durdurulurken beklenmeyen hata.");
            }
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            var configuration = settings.Value;

            // Uygulama açılışında migration/seed işlemlerinin tamamlanması beklenir; ayrıca
            // ilk saniyelerdeki yeniden başlatma döngülerinde gereksiz çalışmayı önler.
            var delay = TimeSpan.FromSeconds(Math.Max(0, configuration.ReconcileStartupDelaySeconds));

            try
            {
                await Task.Delay(delay, cancellationToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }

            using var timer = new PeriodicTimer(
                TimeSpan.FromMinutes(Math.Max(1, configuration.ReconcileIntervalMinutes)));

            while (!cancellationToken.IsCancellationRequested)
            {
                await RunOnceAsync(cancellationToken);

                try
                {
                    if (!await timer.WaitForNextTickAsync(cancellationToken))
                    {
                        break;
                    }
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private async Task RunOnceAsync(CancellationToken cancellationToken)
        {
            try
            {
                // Hosted service singleton, DbContext ve SiteNginxConfigService ise scoped olduğu
                // için scope açmak zorunludur. Servisleri constructor'dan enjekte etmek
                // "captive dependency" hatasına yol açar (ValidateScopes açıkken uygulama başlamaz).
                using var scope = scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var siteNginxConfigService = scope.ServiceProvider.GetRequiredService<SiteNginxConfigService>();

                var activeSiteAlanAdlari = await dbContext.Siteler
                    .AsNoTracking()
                    .Where(site => !site.IsDeleted)
                    .Select(site => site.SiteAlanAdi)
                    .ToListAsync(cancellationToken);

                var changed = await siteNginxConfigService.ReconcileAsync(activeSiteAlanAdlari, cancellationToken);

                logger.LogInformation(
                    "Nginx conf reconcile tamamlandı. Aktif site sayısı: {SiteCount}, Değişiklik: {Changed}",
                    activeSiteAlanAdlari.Count,
                    changed);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Reconcile hatası uygulamayı durdurmamalıdır; sonraki turda tekrar denenir.
                logger.LogError(ex, "Nginx conf reconcile sırasında hata oluştu.");
            }
        }

        public void Dispose()
        {
            _stopSource?.Dispose();
        }
    }
}
