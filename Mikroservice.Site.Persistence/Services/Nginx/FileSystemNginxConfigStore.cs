using Microsoft.Extensions.Logging;

namespace Mikroservice.Site.Persistence.Services.Nginx
{
    /// <summary>
    /// nginx yapılandırmasını dosya sistemine yazan varsayılan depo.
    /// <para>
    /// Yazma işlemi önce hedefle aynı birimdeki bir geçici dosyaya yapılır, ardından
    /// <see cref="File.Move(string, string, bool)"/> ile hedefe taşınır. Aynı birimdeki
    /// taşıma işlemi işletim sistemi düzeyinde atomiktir; böylece nginx hiçbir zaman
    /// yarım yazılmış bir yapılandırma görmez.
    /// </para>
    /// </summary>
    public class FileSystemNginxConfigStore
    (
        ILogger<FileSystemNginxConfigStore> logger
    ) : INginxConfigStore
    {
        public bool Exists(string path) => File.Exists(path);

        public async Task<string?> ReadAsync(string path, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(path))
            {
                return null;
            }

            return await File.ReadAllTextAsync(path, cancellationToken);
        }

        public async Task<bool> WriteAtomicAsync(
            string path,
            string content,
            CancellationToken cancellationToken = default)
        {
            // İçerik aynıysa dosyaya dokunma: gereksiz yazma, gereksiz nginx reload ve
            // mtime değişiminden kaynaklı yanlış "drift" alarmlarını önler.
            if (await IsSameContentAsync(path, content, cancellationToken))
            {
                logger.LogDebug("Nginx conf içeriği değişmedi, yazma atlandı: {ConfPath}", path);
                return false;
            }

            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Geçici dosya hedef dosyayla aynı klasörde olmalı ki taşıma atomik olsun.
            // ".tmp" öneki, nginx'in "include conf.d/*.conf" desenine yakalanmamasını garanti eder.
            var tempPath = $"{path}.{Guid.NewGuid():N}.tmp";

            try
            {
                await File.WriteAllTextAsync(tempPath, content, new System.Text.UTF8Encoding(false), cancellationToken);
                File.Move(tempPath, path, overwrite: true);
                return true;
            }
            catch
            {
                TryDelete(tempPath);
                throw;
            }
        }

        public Task<bool> DeleteAsync(string path, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(path))
            {
                return Task.FromResult(false);
            }

            File.Delete(path);
            return Task.FromResult(true);
        }

        private static async Task<bool> IsSameContentAsync(
            string path,
            string content,
            CancellationToken cancellationToken)
        {
            if (!File.Exists(path))
            {
                return false;
            }

            var existing = await File.ReadAllTextAsync(path, cancellationToken);
            return string.Equals(existing, content, StringComparison.Ordinal);
        }

        private void TryDelete(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Geçici nginx conf dosyası silinemedi: {TempPath}", path);
            }
        }
    }
}
