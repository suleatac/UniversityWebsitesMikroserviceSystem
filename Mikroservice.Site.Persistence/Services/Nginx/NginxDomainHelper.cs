using System.Text.RegularExpressions;

namespace Mikroservice.Site.Persistence.Services.Nginx
{
    /// <summary>
    /// Doğrulanmış site alan adı bilgisi.
    /// </summary>
    /// <param name="Subdomain">Kök alan adı çıkarılmış alt alan adı (ör. "muhendislik" veya "muhendislik.omu").</param>
    /// <param name="FullyQualifiedDomain">nginx server_name için tam alan adı (ör. "muhendislik.sivas.edu.tr").</param>
    /// <param name="ConfFileName">Üretilecek conf dosyasının adı (ör. "muhendislik.sivas.edu.tr.conf").</param>
    public sealed record NormalizedDomain(string Subdomain, string FullyQualifiedDomain, string ConfFileName);

    /// <summary>
    /// Kullanıcıdan gelen alan adının nginx yapılandırmasına güvenli biçimde yazılmasını sağlar.
    /// <para>
    /// Amaç: config injection (<c>;</c>, <c>{</c>, <c>}</c>, satır sonu, boşluk) ve
    /// path traversal (<c>..</c>, <c>/</c>) yoluyla nginx yapılandırmasının bozulmasını engellemek.
    /// Boşluklu/kontrol karakterli bir alan adı, <c>server_name</c> direktifinden kaçıp
    /// yeni direktifler enjekte edebilir; bu nedenle tek savunma hattı katı beyaz liste doğrulamasıdır.
    /// </para>
    /// </summary>
    public static class NginxDomainHelper
    {
        // RFC 1123 etiketi: harf/rakam ile başlar ve biter, ortada tire olabilir, en fazla 63 karakter.
        // Yalnızca küçük harf, rakam, tire ve nokta kabul edilir.
        private static readonly Regex DnsLabelPattern = new(
            "^[a-z0-9]([a-z0-9\\-]{0,61}[a-z0-9])?$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        private const int MaxSubdomainLength = 63;
        private const int MaxFqdnLength = 253;

        /// <summary>
        /// Alan adını doğrular ve normalleştirir. Doğrulama başarısızsa hiçbir dosya yazılmamalıdır.
        /// </summary>
        /// <param name="siteAlanAdi">Kullanıcının girdiği alan adı (alt alan adı veya tam alan adı).</param>
        /// <param name="domainSuffix">Eklenecek kök alan adı (ör. "sivas.edu.tr").</param>
        /// <param name="domain">Doğrulanmış sonuç.</param>
        /// <param name="error">Hata mesajı (başarısızlık durumunda).</param>
        public static bool TryNormalize(
            string? siteAlanAdi,
            string? domainSuffix,
            out NormalizedDomain? domain,
            out string? error)
        {
            domain = null;
            error = null;

            if (string.IsNullOrWhiteSpace(siteAlanAdi))
            {
                error = "Site alan adı boş olamaz.";
                return false;
            }

            // Baştaki/sondaki boşluk ve nokta temizlenir; karşılaştırmalar küçük harf üzerinden yapılır.
            var candidate = siteAlanAdi.Trim().Trim('.').ToLowerInvariant();
            var suffix = (domainSuffix ?? string.Empty).Trim().Trim('.').ToLowerInvariant();

            if (candidate.Length == 0)
            {
                error = "Site alan adı yalnızca nokta karakterinden oluşamaz.";
                return false;
            }

            if (candidate.Contains("..", StringComparison.Ordinal))
            {
                error = $"'{siteAlanAdi}' geçerli bir alan adı değil (ardışık nokta içeriyor).";
                return false;
            }

            // Tam alan adı verilmişse kök alan adını ayıkla (ör. "muhendislik.sivas.edu.tr" -> "muhendislik").
            if (suffix.Length > 0)
            {
                if (candidate.Equals(suffix, StringComparison.Ordinal))
                {
                    error = "Alan adı kök alan adından (domainSuffix) farklı olmalıdır.";
                    return false;
                }

                var suffixWithDot = "." + suffix;
                if (candidate.EndsWith(suffixWithDot, StringComparison.Ordinal))
                {
                    candidate = candidate[..^suffixWithDot.Length];
                }
            }

            if (candidate.Length == 0 || candidate.Length > MaxSubdomainLength)
            {
                error = $"'{siteAlanAdi}' uzunluğu geçersiz (1-{MaxSubdomainLength} karakter olmalı).";
                return false;
            }

            foreach (var label in candidate.Split('.'))
            {
                if (!DnsLabelPattern.IsMatch(label))
                {
                    error = $"'{siteAlanAdi}' geçerli bir alan adı değil. Yalnızca a-z, 0-9 ve '-' kullanılabilir.";
                    return false;
                }
            }

            var fullyQualifiedDomain = suffix.Length > 0 ? $"{candidate}.{suffix}" : candidate;

            if (fullyQualifiedDomain.Length > MaxFqdnLength)
            {
                error = $"'{fullyQualifiedDomain}' alan adı çok uzun (en fazla {MaxFqdnLength} karakter).";
                return false;
            }

            domain = new NormalizedDomain(candidate, fullyQualifiedDomain, fullyQualifiedDomain + ".conf");
            return true;
        }
    }
}
