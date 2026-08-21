namespace Microservice.Web.ViewModels.SeoMetadata
{
    public class SeoMetadataVm
    {
        /// <summary>
        /// Benzersiz anahtar.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// HTML title etiketi için kullanılacak başlık.
        /// </summary>
        public string? MetaTitle { get; set; }

        /// <summary>
        /// HTML meta description etiketi için açıklama.
        /// </summary>
        public string? MetaDescription { get; set; }

        /// <summary>
        /// HTML meta keywords etiketi için anahtar kelimeler.
        /// </summary>
        public string? MetaKeywords { get; set; }

        /// <summary>
        /// Canonical URL.
        /// </summary>
        public string? CanonicalUrl { get; set; }

        /// <summary>
        /// Robots meta değeri.
        /// Örneğin: index, follow veya noindex, nofollow.
        /// </summary>
        public string? Robots { get; set; }

        /// <summary>
        /// Open Graph başlığı.
        /// </summary>
        public string? OgTitle { get; set; }

        /// <summary>
        /// Open Graph açıklaması.
        /// </summary>
        public string? OgDescription { get; set; }

        /// <summary>
        /// Open Graph görsel URL'si.
        /// </summary>
        public string? OgImage { get; set; }

        /// <summary>
        /// Sayfanın arama motorları tarafından indekslenip indekslenemeyeceğini belirtir.
        /// </summary>
        public bool IsIndexable { get; set; }

        /// <summary>
        /// Sayfadaki bağlantıların arama motorları tarafından takip edilip edilmeyeceğini belirtir.
        /// </summary>
        public bool IsFollowable { get; set; }


    }
}
