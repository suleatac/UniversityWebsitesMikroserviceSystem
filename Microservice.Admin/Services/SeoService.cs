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
            Action<string?> setSeoUrl,
            Action<string?>? setSeoTitle = null,
            Action<string?>? setSeoDescription = null,
            string fallbackSlug = "icerik",
            int? excludeIcerikId = null)
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
                var isTaken = await IsSeoUrlTakenAsync(siteId, pageTypeId, candidate, excludeIcerikId);
                if (!isTaken)
                    break;

                occurrence++;
                candidate = SeoHelper.ApplySuffix(baseSlug, occurrence);
            }

            setSeoUrl(candidate);
        }

        /// <summary>
        /// Slug site genelinde kullanilip kullanilmadigini sorgular.
        /// Kontrol servisine erisilemezse kaydin kaydedilmesini engellememek icin
        /// "kullanilabilir" varsayilir (fail-open); buyuk carpisma DB tarafinda yakalanir.
        /// </summary>
        private async Task<bool> IsSeoUrlTakenAsync(int siteId, int pageTypeId, string seoUrl, int? excludeIcerikId)
        {
            try
            {
                var response = await seoCheckClient.IsSeoUrlAvailableAsync(siteId, pageTypeId, seoUrl, excludeIcerikId);

                // Content true => kullanilabilir (bos), false => alinmis.
                if (response.IsSuccessStatusCode && response.Content)
                    return !response.Content;

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
