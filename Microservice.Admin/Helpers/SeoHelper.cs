using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Microservice.Admin.Helpers
{
    /// <summary>
    /// Baslik ve kisa aciklamadan SEO alanlarini (SeoUrl, SeoTitle, SeoDescription)
    /// otomatik ureten yardimci sinif. Kullanicidan SEO alani alinmaz; boylece
    /// hatali/eksik giris onlenir.
    /// </summary>
    public static class SeoHelper
    {
        private const int SeoUrlMaxLength = 200;
        private const int SeoTitleMaxLength = 200;
        private const int SeoDescriptionMaxLength = 500;

        // Turkce karakter karsiliklari
        private static readonly Dictionary<char, string> TurkishCharMap = new()
        {
            { 'ç', "c" }, { 'Ç', "c" },
            { 'ğ', "g" }, { 'Ğ', "g" },
            { 'ı', "i" }, { 'İ', "i" },
            { 'ö', "o" }, { 'Ö', "o" },
            { 'ş', "s" }, { 'Ş', "s" },
            { 'ü', "u" }, { 'Ü', "u" }
        };

        /// <summary>
        /// Basligi SEO dostu sluge cevirir: kucuk harf, Turkce karakterler sadelestirilir,
        /// alfanumerik olmayanlar tire ile degistirilir, fazla tireler birlestirilir.
        /// </summary>
        public static string Slugify(string? text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var sb = new StringBuilder(text.Length);

            foreach (var ch in text)
            {
                if (TurkishCharMap.TryGetValue(ch, out var replacement))
                {
                    sb.Append(replacement);
                    continue;
                }

                // NFD normalizasyonu ile aksanli karakterleri (é, ä vb.) sadelestir
                var normalized = ch.ToString().Normalize(NormalizationForm.FormD);
                var core = normalized
                    .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    .ToArray();

                var candidate = core.Length > 0 ? new string(core) : normalized;

                if (candidate.Length == 1 && char.IsLetterOrDigit(candidate[0]))
                    sb.Append(char.ToLowerInvariant(candidate[0]));
                else
                    sb.Append('-');
            }

            var slug = sb.ToString();

            // Birden fazla tire ve kenar bosluk/tire temizligi
            slug = Regex.Replace(slug, "-{2,}", "-").Trim('-');

            if (slug.Length > SeoUrlMaxLength)
            {
                slug = slug.Substring(0, SeoUrlMaxLength).Trim('-');
            }

            return slug;
        }

        /// <summary>
        /// Baslik ve kisa aciklamadan SEO title uretir.
        /// </summary>
        public static string BuildSeoTitle(string? baslik)
        {
            var title = (baslik ?? string.Empty).Trim();

            if (title.Length > SeoTitleMaxLength)
                title = title.Substring(0, SeoTitleMaxLength).Trim();

            return title;
        }

        /// <summary>
        /// Kisa aciklamadan (yoksa basliktan) SEO aciklamasi uretir.
        /// </summary>
        public static string BuildSeoDescription(string? baslik, string? kisaAciklama)
        {
            var description = !string.IsNullOrWhiteSpace(kisaAciklama)
                ? kisaAciklama.Trim()
                : (baslik ?? string.Empty).Trim();

            if (description.Length > SeoDescriptionMaxLength)
                description = description.Substring(0, SeoDescriptionMaxLength).Trim();

            return description;
        }

        /// <summary>
        /// Slug'a benzersizlik eki uygular: "baslik", "baslik-2", "baslik-3" ...
        /// </summary>
        public static string ApplySuffix(string slug, int occurrence)
        {
            if (occurrence <= 1)
                return slug;

            var suffix = $"-{occurrence}";

            // Max uzunlugu asmamak icin slug'in sonundan kes
            if (slug.Length + suffix.Length > SeoUrlMaxLength)
                slug = slug.Substring(0, SeoUrlMaxLength - suffix.Length).Trim('-');

            return slug + suffix;
        }
    }
}
