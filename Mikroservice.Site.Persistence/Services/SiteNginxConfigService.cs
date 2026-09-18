using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mikroservice.Site.Persistence.Services.Nginx;
using Mikroservice.Site.Persistence.Settings;

namespace Mikroservice.Site.Persistence.Services
{
    /// <summary>
    /// Site alan adları için nginx yapılandırmasını üretir, yazar ve temizler.
    /// <para>
    /// Sorumluluklar:
    /// <list type="bullet">
    /// <item><description><b>Doğrulama:</b> alan adı beyaz liste ile doğrulanmadan hiçbir dosya yazılmaz.</description></item>
    /// <item><description><b>Snippet mimarisi:</b> paylaşılan location blokları conf.d köküne değil, <c>snippets/</c>
    /// altına yazılır. conf.d kökündeki dosyalar nginx tarafından <b>http</b> seviyesinde include edildiği için
    /// orada bir <c>location</c> bloğu bulunması yapılandırmayı bozar; subdirectory'deki dosyalar glob ile
    /// eşleşmediğinden güvenlidir.</description></item>
    /// <item><description><b>Atomiklik ve idempotency:</b> yazma işi <see cref="INginxConfigStore"/> üzerinden
    /// atomik yapılır, içerik değişmediyse dosyaya dokunulmaz.</description></item>
    /// <item><description><b>Eşzamanlılık:</b> tüm değişiklikler dağıtık kilit altında yapılır.</description></item>
    /// <item><description><b>Şablon sürümü:</b> üretilen dosyanın başına <c># template-version</c> yazılır;
    /// reconcile bu sayede şablonu eskimiş dosyaları yeniler.</description></item>
    /// </list>
    /// </para>
    /// </summary>
    public class SiteNginxConfigService
    (
        INginxConfigStore configStore,
        INginxConfigLockProvider lockProvider,
        INginxReloader reloader,
        IOptions<NginxConfigSetting> settings,
        ILogger<SiteNginxConfigService> logger
    )
    {
        private const string ManagedHeaderMarker = "# Managed by UniversityWebsitesMikroserviceSystem";
        private const string TemplateVersionPrefix = "# template-version:";
        private const string SitePrefix = "# site:";

        // conf.d kökündeki tüm mutasyonlar aynı kilit altında yapılır. Site oluşturma nadir bir işlem
        // olduğu için paralellik kaybı önemsizdir; buna karşılık reconcile ile site yazmasının
        // birbiriyle yarışması (dosyanın silinip hemen yeniden yazılması) tamamen engellenmiş olur.
        private const string GlobalLockResource = "nginx-conf";

        public async Task ApplyAsync(
            string siteAlanAdi,
            string? previousSiteAlanAdi,
            bool isDeleted,
            CancellationToken cancellationToken = default)
        {
            var configuration = settings.Value;

            if (isDeleted)
            {
                await RemoveAsync(siteAlanAdi, previousSiteAlanAdi, configuration, cancellationToken);
                return;
            }

            if (!NginxDomainHelper.TryNormalize(siteAlanAdi, configuration.DomainSuffix, out var domain, out var error))
            {
                // Güvenlik: doğrulanmamış bir alan adı nginx yapılandırmasına asla yazılmaz.
                logger.LogError(
                    "Site alan adı doğrulanamadı, nginx yapılandırması üretilmedi. Alan adı: {SiteAlanAdi}. Hata: {Error}",
                    siteAlanAdi,
                    error);

                return;
            }

            await using var @lock = await AcquireLockAsync(configuration, cancellationToken);

            if (@lock is null)
            {
                logger.LogWarning("Nginx conf kilidi alınamadı; site {SiteAlanAdi} için yazma atlandı.", domain!.FullyQualifiedDomain);
                return;
            }

            await EnsureSnippetsAsync(configuration, cancellationToken);

            var written = await WriteSiteConfigAsync(domain!, configuration, cancellationToken);

            // Alan adı değiştiyse eski dosya kalıcı olarak silinmelidir; aksi hâlde nginx
            // aynı siteyi iki server_name ile eşleştirir ve yanlış içerik sunar.
            if (!string.IsNullOrWhiteSpace(previousSiteAlanAdi) &&
                !string.Equals(previousSiteAlanAdi, siteAlanAdi, StringComparison.OrdinalIgnoreCase))
            {
                await RemoveConfigByDomainAsync(previousSiteAlanAdi!, configuration, cancellationToken);
            }

            if (written)
            {
                await reloader.RequestReloadAsync(cancellationToken);
            }
        }

        /// <summary>
        /// Veritabanındaki gerçek duruma göre conf.d klasörünü mutabık kılar:
        /// <list type="bullet">
        /// <item><description>Veritabanında olmayan site dosyaları (orphan) silinir.</description></item>
        /// <item><description>Şablon sürümü eskimiş dosyalar yeniden üretilir.</description></item>
        /// <item><description>Eksik site dosyaları oluşturulur.</description></item>
        /// </list>
        /// </summary>
        /// <returns>Değişiklik yapıldıysa true.</returns>
        public async Task<bool> ReconcileAsync(
            IReadOnlyCollection<string> activeSiteAlanAdlari,
            CancellationToken cancellationToken = default)
        {
            var configuration = settings.Value;

            var desired = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var alanAdi in activeSiteAlanAdlari)
            {
                if (NginxDomainHelper.TryNormalize(alanAdi, configuration.DomainSuffix, out var domain, out var error))
                {
                    desired[domain!.FullyQualifiedDomain] = alanAdi;
                }
                else
                {
                    // Veritabanındaki geçersiz kayıtları sessizce geçiyoruz; her turda log basılmasın diye Debug seviyesi.
                    logger.LogDebug("Reconcile sırasında geçersiz alan adı atlandı: {SiteAlanAdi}. Hata: {Error}", alanAdi, error);
                }
            }

            await using var @lock = await AcquireLockAsync(configuration, cancellationToken);

            if (@lock is null)
            {
                logger.LogWarning("Reconcile için nginx conf kilidi alınamadı; tur atlandı.");
                return false;
            }

            var changed = false;

            // Snippet'ler her turda garanti altına alınır (klasör yanlışlıkla silinirse kendini onarır).
            await EnsureSnippetsAsync(configuration, cancellationToken);

            var existing = GetExistingManagedConfigFiles(configuration);

            foreach (var path in existing)
            {
                var managed = await TryReadManagedHeaderAsync(path, cancellationToken);

                if (managed is null)
                {
                    // Yönetim başlığı yoksa bu dosya ya bu uygulamaya ait değildir (elle eklenmiş,
                    // yönetim dışı bir server bloğu) ya da eski sürüm tarafından üretilmiştir.
                    // Eski sürümün ürettiği dosyanın adı tam olarak "<fqdn>.conf" olduğu için,
                    // adı veritabanındaki bir siteyle eşleşiyorsa dosya legacy kabul edilir ve
                    // yeni şablona göre yeniden yazılır. Aksi hâlde (yönetim dışı) dokunulmaz.
                    var mappedDomain = desired.Keys.FirstOrDefault(candidate =>
                        string.Equals(path, GetConfPath(configuration, candidate), StringComparison.OrdinalIgnoreCase));

                    if (mappedDomain is null)
                    {
                        logger.LogDebug("Yönetilmeyen conf dosyası atlandı (başlık bulunamadı): {ConfPath}", path);
                        continue;
                    }

                    logger.LogInformation(
                        "Reconcile: eski şablonda üretilmiş conf yeni şablona taşınıyor. Site: {Site}",
                        mappedDomain);

                    if (NginxDomainHelper.TryNormalize(desired[mappedDomain], configuration.DomainSuffix, out var legacyDomain, out _))
                    {
                        changed |= await WriteSiteConfigAsync(legacyDomain!, configuration, cancellationToken);
                    }

                    continue;
                }

                if (!desired.ContainsKey(managed.Site))
                {
                    logger.LogInformation(
                        "Reconcile: veritabanında karşılığı olmayan nginx conf siliniyor. Dosya: {ConfPath}, Site: {Site}",
                        path,
                        managed.Site);

                    await configStore.DeleteAsync(path, cancellationToken);
                    changed = true;
                    continue;
                }

                if (managed.TemplateVersion < configuration.TemplateVersion)
                {
                    logger.LogInformation(
                        "Reconcile: şablon sürümü eskimiş conf yeniden üretiliyor. Site: {Site}, Mevcut: {Current}, Beklenen: {Expected}",
                        managed.Site,
                        managed.TemplateVersion,
                        configuration.TemplateVersion);

                    if (NginxDomainHelper.TryNormalize(desired[managed.Site], configuration.DomainSuffix, out var domain, out _))
                    {
                        changed |= await WriteSiteConfigAsync(domain!, configuration, cancellationToken);
                    }
                }
            }

            // Eksik dosyaları üret.
            foreach (var (fullyQualifiedDomain, _) in desired)
            {
                var path = GetConfPath(configuration, fullyQualifiedDomain);

                if (!configStore.Exists(path))
                {
                    logger.LogInformation("Reconcile: eksik nginx conf üretiliyor. Site: {Site}", fullyQualifiedDomain);

                    if (NginxDomainHelper.TryNormalize(fullyQualifiedDomain, configuration.DomainSuffix, out var domain, out _))
                    {
                        changed |= await WriteSiteConfigAsync(domain!, configuration, cancellationToken);
                    }
                }
            }

            if (changed)
            {
                await reloader.RequestReloadAsync(cancellationToken);
            }

            return changed;
        }

        private async Task RemoveAsync(
            string? siteAlanAdi,
            string? previousSiteAlanAdi,
            NginxConfigSetting configuration,
            CancellationToken cancellationToken)
        {
            await using var @lock = await AcquireLockAsync(configuration, cancellationToken);

            if (@lock is null)
            {
                logger.LogWarning("Nginx conf kilidi alınamadı; silme işlemi atlandı. Alan adı: {SiteAlanAdi}", siteAlanAdi);
                return;
            }

            var changed = await RemoveConfigByDomainAsync(siteAlanAdi, configuration, cancellationToken);

            if (!string.IsNullOrWhiteSpace(previousSiteAlanAdi))
            {
                changed |= await RemoveConfigByDomainAsync(previousSiteAlanAdi!, configuration, cancellationToken);
            }

            if (changed)
            {
                await reloader.RequestReloadAsync(cancellationToken);
            }
        }

        private async Task<bool> RemoveConfigByDomainAsync(
            string? siteAlanAdi,
            NginxConfigSetting configuration,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(siteAlanAdi))
            {
                return false;
            }

            // Silme yolunda da doğrulama zorunludur: doğrulanmamış bir girdi ("../../etc/passwd")
            // aksi hâlde conf.d dışında keyfi dosya silinmesine yol açabilir.
            if (!NginxDomainHelper.TryNormalize(siteAlanAdi, configuration.DomainSuffix, out var domain, out var error))
            {
                logger.LogWarning("Silinecek alan adı doğrulanamadı, atlandı. Alan adı: {SiteAlanAdi}. Hata: {Error}", siteAlanAdi, error);
                return false;
            }

            var path = GetConfPath(configuration, domain!.FullyQualifiedDomain);
            var deleted = await configStore.DeleteAsync(path, cancellationToken);

            if (deleted)
            {
                logger.LogInformation("Nginx conf silindi: {ConfPath}", path);
            }
            else
            {
                logger.LogDebug("Silinecek nginx conf bulunamadı: {ConfPath}", path);
            }

            return deleted;
        }

        private async Task<bool> WriteSiteConfigAsync(
            NormalizedDomain domain,
            NginxConfigSetting configuration,
            CancellationToken cancellationToken)
        {
            var path = GetConfPath(configuration, domain.FullyQualifiedDomain);

            // Reconcile her turda bu yolu çağırdığı için dosya değişmediyse yazma yapılmaz (idempotency).
            var written = await configStore.WriteAtomicAsync(
                path,
                BuildConfigContent(domain, configuration),
                cancellationToken);

            if (written)
            {
                logger.LogInformation("Nginx conf yazıldı: {ConfPath}", path);
            }

            return written;
        }

        /// <summary>
        /// Paylaşılan snippet dosyalarını yazar. Snippet'ler conf.d kökünde değil
        /// <c>snippets/</c> altında tutulur; böylece nginx'in http seviyesindeki
        /// <c>include conf.d/*.conf</c> direktifine takılmazlar.
        /// </summary>
        private async Task EnsureSnippetsAsync(
            NginxConfigSetting configuration,
            CancellationToken cancellationToken)
        {
            var snippetDirectory = GetSnippetDirectory(configuration);
            Directory.CreateDirectory(snippetDirectory);

            await configStore.WriteAtomicAsync(
                Path.Combine(snippetDirectory, "media-location.conf"),
                BuildMediaLocationSnippet(configuration),
                cancellationToken);
        }

        private static string BuildMediaLocationSnippet(NginxConfigSetting configuration)
        {
            return $$"""
            {{ManagedHeaderMarker}}
            # Admin panelinden yuklenen medya dosyalari (MinIO).
            # Iceriklerdeki goreceli /media/... yollari buradan ic agdaki MinIO'ya gider;
            # MinIO dis DNS'e asla acilmaz.
            location /media/ {
                proxy_pass {{configuration.MediaProxyPassUrl}};
                proxy_buffering off;
                expires 30d;
                add_header Cache-Control "public, max-age=2592000";
            }
            """;
        }

        private static string BuildConfigContent(NormalizedDomain domain, NginxConfigSetting configuration)
        {
            var snippetIncludePath =
                $"{configuration.ContainerConfDirectory.TrimEnd('/')}/snippets/media-location.conf";

            return $$"""
            {{ManagedHeaderMarker}}
            # template-version: {{configuration.TemplateVersion}}
            # site: {{domain.FullyQualifiedDomain}}

            server {
                listen 80;
                server_name {{domain.FullyQualifiedDomain}};

                return 301 https://$host$request_uri;
            }

            server {
                listen 443 ssl;
                server_name {{domain.FullyQualifiedDomain}};

                ssl_certificate     {{configuration.CertificateDirectory}}/{{configuration.CertificateBaseName}}.crt;
                ssl_certificate_key {{configuration.CertificateDirectory}}/{{configuration.CertificateBaseName}}.key;

                ssl_protocols TLSv1.2 TLSv1.3;

                # Medya (MinIO) location blogu ortak snippet'ten gelir.
                include {{snippetIncludePath}};

                location / {
                    proxy_pass {{configuration.ProxyPassUrl}};
                    proxy_set_header Host $host;
                    proxy_set_header X-Real-IP $remote_addr;
                    proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
                    proxy_set_header X-Forwarded-Proto https;
                }
            }
            """;
        }

        private static string GetConfPath(NginxConfigSetting configuration, string fullyQualifiedDomain)
            => Path.Combine(configuration.ConfDirectory, fullyQualifiedDomain + ".conf");

        private static string GetSnippetDirectory(NginxConfigSetting configuration)
            => Path.Combine(configuration.ConfDirectory, configuration.SnippetsDirectoryName);

        /// <summary>
        /// conf.d kökündeki, bu uygulama tarafından üretilmiş (<c>.conf</c>) dosyaları döner.
        /// Snippet klasörü, geçici ve yedek dosyalar kapsam dışıdır.
        /// </summary>
        private IEnumerable<string> GetExistingManagedConfigFiles(NginxConfigSetting configuration)
        {
            if (!Directory.Exists(configuration.ConfDirectory))
            {
                return [];
            }

            return Directory
                .EnumerateFiles(configuration.ConfDirectory, "*.conf", SearchOption.TopDirectoryOnly)
                .Where(path => !path.EndsWith(".tmp", StringComparison.OrdinalIgnoreCase))
                .Where(path => !path.EndsWith(".bak", StringComparison.OrdinalIgnoreCase));
        }

        private async Task<ManagedConfigHeader?> TryReadManagedHeaderAsync(
            string path,
            CancellationToken cancellationToken)
        {
            try
            {
                var content = await configStore.ReadAsync(path, cancellationToken);

                if (string.IsNullOrEmpty(content) || !content.Contains(ManagedHeaderMarker, StringComparison.Ordinal))
                {
                    return null;
                }

                var templateVersion = 0;
                var site = string.Empty;

                // Başlık her zaman dosyanın en başında olduğu için tüm dosyayı satır satır gezmek
                // yerine yalnızca ilk birkaç satıra bakmak yeterlidir.
                using var reader = new StringReader(content);

                for (var index = 0; index < 10; index++)
                {
                    var line = await reader.ReadLineAsync(cancellationToken);

                    if (line is null)
                    {
                        break;
                    }

                    if (line.StartsWith(TemplateVersionPrefix, StringComparison.Ordinal))
                    {
                        int.TryParse(line[TemplateVersionPrefix.Length..].Trim(), out templateVersion);
                    }
                    else if (line.StartsWith(SitePrefix, StringComparison.Ordinal))
                    {
                        site = line[SitePrefix.Length..].Trim();
                    }
                }

                if (string.IsNullOrWhiteSpace(site))
                {
                    return null;
                }

                return new ManagedConfigHeader(site, templateVersion);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Nginx conf başlığı okunamadı: {ConfPath}", path);
                return null;
            }
        }

        private async Task<IAsyncDisposable?> AcquireLockAsync(
            NginxConfigSetting configuration,
            CancellationToken cancellationToken)
        {
            return await lockProvider.AcquireAsync(
                GlobalLockResource,
                TimeSpan.FromSeconds(Math.Max(5, configuration.LockTimeoutSeconds)),
                TimeSpan.FromSeconds(Math.Max(1, configuration.LockWaitSeconds)),
                cancellationToken);
        }

        private sealed record ManagedConfigHeader(string Site, int TemplateVersion);
    }
}
