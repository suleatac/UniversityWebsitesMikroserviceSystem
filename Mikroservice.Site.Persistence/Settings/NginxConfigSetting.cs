namespace Mikroservice.Site.Persistence.Settings
{
    /// <summary>
    /// nginx yapılandırma üretimini kontrol eden ayarlar.
    /// appsettings.json içindeki "SiteNginxConfig" bölümünden bağlanır.
    /// <para>
    /// Sabitlerin (dizin, sertifika adı, proxy hedefi) koda gömülü olması yerine
    /// konfigürasyondan okunması, farklı ortamlarda (dev/prod) davranışı değiştirmeyi
    /// ve deploy etmeden ayar güncellemeyi mümkün kılar.
    /// </para>
    /// </summary>
    public class NginxConfigSetting
    {
        public const string SectionName = "SiteNginxConfig";

        /// <summary>Üretilen .conf dosyalarının yazıldığı kök dizin (Site.Api container'ında conf.d ile eşlenir).</summary>
        public string ConfDirectory { get; set; } = "/nginx-conf";

        /// <summary>Yeniden kullanılabilir snippet dosyalarının ConfDirectory altındaki klasör adı.</summary>
        public string SnippetsDirectoryName { get; set; } = "snippets";

        /// <summary>
        /// nginx container'ının gördüğü conf.d dizini. include yolları bu öneke göre üretilir;
        /// ConfDirectory ile aynı host klasörünün farklı bir mount noktasıdır.
        /// </summary>
        public string ContainerConfDirectory { get; set; } = "/etc/nginx/conf.d";

        /// <summary>nginx container'ı içindeki sertifika dizini.</summary>
        public string CertificateDirectory { get; set; } = "/etc/nginx/ssl";

        /// <summary>Sertifika dosyalarının temel adı (ör. sivas.edu.tr.crt / sivas.edu.tr.key).</summary>
        public string CertificateBaseName { get; set; } = "sivas.edu.tr";

        /// <summary>Site alan adlarının eklendiği kök alan adı (ör. "sivas.edu.tr").</summary>
        public string DomainSuffix { get; set; } = "sivas.edu.tr";

        /// <summary>location / için proxy hedefi (genel .NET web uygulaması).</summary>
        public string ProxyPassUrl { get; set; } = "http://microservice.web:8080";

        /// <summary>location /media/ için proxy hedefi (iç ağdaki MinIO). MinIO dış DNS'e açılmaz.</summary>
        public string MediaProxyPassUrl { get; set; } = "http://minio:9000/site-media/";

        /// <summary>
        /// Üretilen şablonun sürümü. Değer artırıldığında reconcile işi tüm dosyaları
        /// yeniden üretir; böylece şablon değişiklikleri mevcut dosyalara da yayılır.
        /// </summary>
        public int TemplateVersion { get; set; } = 2;

        /// <summary>
        /// nginx'in bu uygulama tarafından reload edilip edilmeyeceği.
        /// Reload işini harici bir servis yapıyorsa false bırakılmalıdır.
        /// </summary>
        public bool EnableConfigReload { get; set; }

        /// <summary>Reload/doğrulama komutunun çalıştırılabilir dosyası (ör. "docker").</summary>
        public string? ReloadCommand { get; set; }

        /// <summary>Reload komutunun argümanları (ör. "exec nginx nginx -s reload").</summary>
        public string? ReloadArguments { get; set; }

        /// <summary>Reload öncesi doğrulama komutu (ör. "docker", argüman "exec nginx nginx -t"). Boşsa atlanır.</summary>
        public string? ValidateCommand { get; set; }

        /// <summary>Doğrulama komutunun argümanları.</summary>
        public string? ValidateArguments { get; set; }

        /// <summary>Ardışık reload isteklerini birleştirmek için beklenecek süre (saniye).</summary>
        public int ReloadDebounceSeconds { get; set; } = 3;

        /// <summary>Periyodik mutabakat (reconcile) işi açık mı?</summary>
        public bool EnableReconcile { get; set; } = true;

        /// <summary>Uygulama açılışında reconcile işinin başlamasından önce beklenecek süre (saniye).</summary>
        public int ReconcileStartupDelaySeconds { get; set; } = 20;

        /// <summary>Reconcile döngüsü aralığı (dakika).</summary>
        public int ReconcileIntervalMinutes { get; set; } = 30;

        /// <summary>
        /// Aynı anda birden fazla replikanın yazmasını engellemek için dağıtık kilit kullanılsın mı?
        /// Redis erişilemezse süreç içi (in-process) kilide düşer.
        /// </summary>
        public bool EnableDistributedLock { get; set; } = true;

        /// <summary>Dağıtık kilidin yaşam süresi (saniye).</summary>
        public int LockTimeoutSeconds { get; set; } = 60;

        /// <summary>Kilit alınamazsa beklenecek en uzun süre (saniye).</summary>
        public int LockWaitSeconds { get; set; } = 15;
    }
}
