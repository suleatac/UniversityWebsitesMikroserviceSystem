using Microservice.Admin.Clients.SeoCheckClients;
using Microservice.Admin.Helpers;
using Microservice.Admin.Services.Interfaces;

namespace Microservice.Admin.Services
{
    /// <inheritdoc />
    public class SeoService(ISeoCheckClientServices seoCheckClient) : ISeoService
    {
        private const int MaxAttempts = 50;

        /// <inheritdoc />
        public async Task ApplyAutoSeoAsync(
            int siteId,
            int pageTypeId,
            string? baslik,
            string? kisaAciklama,
            Action<string> setSeoUrl,
            Action<string?>? setSeoTitle = null,
            Action<string?>? setSeoDescription = null,
            string fallbackSlug = "icerik",
            int? excludeIcerikId = null,
            int? excludeSitePersonelId = null)
        {
            setSeoTitle?.Invoke(SeoHelper.BuildSeoTitle(baslik));
            setSeoDescription?.Invoke(SeoHelper.BuildSeoDescription(baslik, kisaAciklama));

            var baseSlug = SeoHelper.Slugify(baslik);
            if (string.IsNullOrWhiteSpace(baseSlug))
                baseSlug = fallbackSlug;

            // Baslik degismediginde guncelleme sirasinda kayd'in kendi slug'i
            // excludeId sayesinde carpisma sayilmaz ve slug sabit kalir.
            var candidate = baseSlug;
            var occurrence = 1;

            while (occurrence <= MaxAttempts)
            {
                var isTaken = await IsSeoUrlTakenAsync(siteId, pageTypeId, candidate, excludeIcerikId, excludeSitePersonelId);
                if (!isTaken)
                    break;

                occurrence++;
                candidate = SeoHelper.ApplySuffix(baseSlug, occurrence);
            }

            setSeoUrl(candidate);
        }

        /// <summary>
        /// Slug site genelinde kullanilip kullanilmadigini sorgular.
        /// Kontrol servisine hic erisilemezse (exception / HTTP hatasi) kaydin kaydedilmesini
        /// engellememek icin "kullanilabilir" varsayilir (fail-open); buyuk carpisma DB tarafinda
        /// 409 olarak yakalanir. ANCAK sunucu "alinmis" (Content=false) dediyse bu cevap
        /// kesinlikle "taken" sayilmalidir - eski kod bunu yanlislikla fail-open dalinda
        /// "kullanilabilir" yorumluyor ve DB'ye 23505 unique violation gidiyordu.
        /// </summary>
        private async Task<bool> IsSeoUrlTakenAsync(int siteId, int pageTypeId, string seoUrl, int? excludeIcerikId, int? excludeSitePersonelId)
        {
            try
            {
                var response = await seoCheckClient.IsSeoUrlAvailableAsync(siteId, pageTypeId, seoUrl, excludeIcerikId, excludeSitePersonelId);

                // Gecerli cevap alinamadiysa fail-open: kullanilabilir varsay.
                if (!response.IsSuccessStatusCode)
                    return false;

                // Content true => kullanilabilir (taken degil), Content false => alinmis (taken).
                return !response.Content;
            }
            catch
            {
                return false;
            }
        }
    }
}
