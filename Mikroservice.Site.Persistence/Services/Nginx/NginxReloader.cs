using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mikroservice.Site.Persistence.Settings;

namespace Mikroservice.Site.Persistence.Services.Nginx
{
    /// <summary>
    /// nginx'i doğrulayarak (nginx -t) reload eden arka plan çalışanı.
    /// <para>
    /// İki koruma sağlar:
    /// <list type="number">
    /// <item><description><b>Debounce:</b> art arda gelen değişiklikler tek reload'da birleştirilir.
    /// Örneğin 10 site birkaç saniye içinde oluşturulursa nginx yalnızca bir kez reload edilir.</description></item>
    /// <item><description><b>Doğrulama:</b> reload öncesi <c>nginx -t</c> çalıştırılır; doğrulama başarısız olursa
    /// reload edilmez ve hata loglanır. Böylece bozuk bir conf dosyası çalışan nginx'i düşürmez.</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Varsayılan olarak <b>kapalıdır</b> (<c>EnableConfigReload=false</c>). Reload işini harici bir
    /// servis yapıyorsa bu sınıf hiçbir komut çalıştırmaz ve arka plan işi <b>hiç başlamaz</b>
    /// (ilk istek üzerine tembel başlatma), dolayısıyla boşuna kaynak tüketilmez.
    /// </para>
    /// </summary>
    public class NginxReloader
    (
        IOptions<NginxConfigSetting> settings,
        ILogger<NginxReloader> logger
    ) : INginxReloader, IAsyncDisposable
    {
        // Kapasitesi 1 olan sinyal: aynı anda en fazla bir bekleyen reload isteği tutulur.
        // Bu, "sürü" halinde gelen isteklerin otomatik olarak tek reload'a birleşmesini sağlar.
        private readonly SemaphoreSlim _reloadSignal = new(0, 1);

        private readonly CancellationTokenSource _stopSource = new();

        private readonly object _workerGate = new();

        private Task? _worker;
        private bool _disposed;

        public Task RequestReloadAsync(CancellationToken cancellationToken = default)
        {
            if (!settings.Value.EnableConfigReload)
            {
                logger.LogDebug("Otomatik nginx reload kapalı (EnableConfigReload=false); istek yok sayıldı.");
                return Task.CompletedTask;
            }

            if (_disposed)
            {
                return Task.CompletedTask;
            }

            EnsureWorkerStarted();

            // Sinyal zaten doluysa (bekleyen bir istek varsa) tekrar Release etmek
            // SemaphoreFullException fırlatır; bu yüzden kontrollü bırakıyoruz.
            if (_reloadSignal.CurrentCount == 0)
            {
                try
                {
                    _reloadSignal.Release();
                }
                catch (SemaphoreFullException)
                {
                    // Yarış durumu: başka bir akış zaten sinyali bıraktı; bekleyen istek mevcut.
                }
            }

            return Task.CompletedTask;
        }

        private void EnsureWorkerStarted()
        {
            if (_worker is not null)
            {
                return;
            }

            lock (_workerGate)
            {
                if (_worker is not null)
                {
                    return;
                }

                _worker = Task.Run(() => RunAsync(_stopSource.Token), CancellationToken.None);

                logger.LogInformation(
                    "nginx reload arka plan işi başlatıldı (debounce: {Seconds}s).",
                    settings.Value.ReloadDebounceSeconds);
            }
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            var debounce = TimeSpan.FromSeconds(Math.Max(1, settings.Value.ReloadDebounceSeconds));

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    // İlk istek gelene kadar bekle.
                    await _reloadSignal.WaitAsync(cancellationToken);

                    // Sakinleşme penceresi: bu süre içinde gelen yeni istekler aynı reload'da birleşir.
                    await Task.Delay(debounce, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                // Debounce sonrasında birikmiş istekleri boşalt (sinyal kapasitesi 1 olduğu için tek kontrol yeterli).
                while (_reloadSignal.Wait(0))
                {
                    // Drain.
                }

                await ValidateAndReloadAsync(cancellationToken);
            }
        }

        private async Task ValidateAndReloadAsync(CancellationToken cancellationToken)
        {
            var configuration = settings.Value;

            // Doğrulama, bozuk conf'un çalışan nginx'i düşürmesini engelleyen kritik adımdır.
            if (!string.IsNullOrWhiteSpace(configuration.ValidateCommand))
            {
                var validation = await RunProcessAsync(
                    configuration.ValidateCommand!,
                    configuration.ValidateArguments,
                    cancellationToken);

                if (!validation.Success)
                {
                    logger.LogError(
                        "nginx yapılandırma doğrulaması başarısız; reload İPTAL edildi. Çıkış kodu: {ExitCode}. Çıktı: {Output}",
                        validation.ExitCode,
                        validation.Output);

                    return;
                }
            }

            if (string.IsNullOrWhiteSpace(configuration.ReloadCommand))
            {
                logger.LogWarning("ReloadCommand tanımlı değil; nginx reload edilemedi.");
                return;
            }

            var reload = await RunProcessAsync(
                configuration.ReloadCommand!,
                configuration.ReloadArguments,
                cancellationToken);

            if (reload.Success)
            {
                logger.LogInformation("nginx başarıyla reload edildi.");
            }
            else
            {
                logger.LogError(
                    "nginx reload başarısız. Çıkış kodu: {ExitCode}. Çıktı: {Output}",
                    reload.ExitCode,
                    reload.Output);
            }
        }

        private async Task<(bool Success, int ExitCode, string Output)> RunProcessAsync(
            string fileName,
            string? arguments,
            CancellationToken cancellationToken)
        {
            try
            {
                var startInfo = new ProcessStartInfo
                {
                    FileName = fileName,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                foreach (var argument in SplitArguments(arguments))
                {
                    startInfo.ArgumentList.Add(argument);
                }

                using var process = Process.Start(startInfo);

                if (process is null)
                {
                    return (false, -1, $"'{fileName}' başlatılamadı.");
                }

                var standardOutput = process.StandardOutput.ReadToEndAsync(cancellationToken);
                var standardError = process.StandardError.ReadToEndAsync(cancellationToken);

                await process.WaitForExitAsync(cancellationToken);

                var output = $"{await standardOutput}{await standardError}".Trim();

                return (process.ExitCode == 0, process.ExitCode, output);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                // Örn. docker CLI container içinde yok veya docker socket mount edilmemiş.
                return (false, -1, ex.Message);
            }
        }

        /// <summary>
        /// Argüman dizesini boşluklara göre ayırır; tırnak içindeki grupları korur.
        /// </summary>
        private static IEnumerable<string> SplitArguments(string? arguments)
        {
            if (string.IsNullOrWhiteSpace(arguments))
            {
                yield break;
            }

            var current = new System.Text.StringBuilder();
            var quote = '\0';

            foreach (var character in arguments)
            {
                if (quote != '\0')
                {
                    if (character == quote)
                    {
                        quote = '\0';
                    }
                    else
                    {
                        current.Append(character);
                    }

                    continue;
                }

                if (character is '"' or '\'')
                {
                    quote = character;
                    continue;
                }

                if (char.IsWhiteSpace(character))
                {
                    if (current.Length > 0)
                    {
                        yield return current.ToString();
                        current.Clear();
                    }

                    continue;
                }

                current.Append(character);
            }

            if (current.Length > 0)
            {
                yield return current.ToString();
            }
        }

        public async ValueTask DisposeAsync()
        {
            // DI konteyneri ve host aynı örneği dispose edebildiği için idempotent olmalıdır.
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            await _stopSource.CancelAsync();

            if (_worker is not null)
            {
                try
                {
                    await _worker.WaitAsync(TimeSpan.FromSeconds(5));
                }
                catch (Exception ex)
                {
                    logger.LogDebug(ex, "nginx reload arka plan işi durdurulurken beklenmeyen hata.");
                }
            }

            _stopSource.Dispose();
            _reloadSignal.Dispose();
        }
    }
}
