namespace Microservice.Admin.Services.Interfaces
{
    /// <summary>
    /// Baslik/kisa aciklamadan SEO alanlarini (SeoUrl, SeoTitle, SeoDescription)
    /// otomatik ureten ortak servis. Kullaniciyi SEO alani girmekten kurtarir;
    /// olusan slug'in site genelinde benzersiz olmasini kontrol eder.
    /// </summary>
    public interface ISeoService
    {
        /// <summary>
        /// SEO alanlarini basliga gore uretip verilen setter'lara atar.
        /// SeoUrl icin site genelinde (tum içerik türleri + SSS) kullanilabilirlik
        /// kontrolu yapilir; carpisma varsa "-2", "-3" ekiyle benzersizlestirilir.
        /// </summary>
        /// <param name="siteId">Kaydin ait oldugu site.</param>
        /// <param name="baslik">Slug ve title kaynagi baslik.</param>
        /// <param name="kisaAciklama">Aciklama kaynagi (bos ise baslik kullanilir).</param>
        /// <param name="setSeoUrl">Uretilen slug'u yazacak setter.</param>
        /// <param name="setSeoTitle">Uretilen title'i yazacak setter (null gecilebilir).</param>
        /// <param name="setSeoDescription">Uretilen aciklamayi yazacak setter (null gecilebilir).</param>
        /// <param name="fallbackSlug">Baslik'tan slug uretilemezse kullanilacak taban slug.</param>
        /// <param name="excludeIcerikId">Guncellemede kendisinin carpisma sayilmamasi icin Icerik Id.</param>
        Task ApplyAutoSeoAsync(
             int siteId,
             int pageTypeId,
             string? baslik,
             string? kisaAciklama,
             Action<string?> setSeoUrl,
             Action<string?>? setSeoTitle = null,
             Action<string?>? setSeoDescription = null,
             string fallbackSlug = "icerik",
             int? excludeIcerikId = null);
    }
}
