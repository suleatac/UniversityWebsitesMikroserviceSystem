using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.IcerikFeatures.IsSeoUrlAvailable
{
    // SeoUrl'un site genelinde (tüm içerik türleri ve diller dahil, Icerik tablosu + SitePersoneller tablosu)
    // kullanilabilir olup olmadigini kontrol eder.
    // exclude* parametreleri: guncelleme sirasinda kayd'in kendisinin carpisma sayilmamasi icin verilir.
    // Not: Benzersizlik index'leri (SiteId, SeoUrl) oldugundan PageTypeId artik filtre olarak KULLANILMAZ;
    // interface uyumu icin tasinmaya devam edebilir ancak sorguda yok sayilir.
    public record IsSeoUrlAvailableQuery(
        int SiteId,
        string SeoUrl,
        int PageTypeId,
        int? ExcludeIcerikId,
        int? ExcludeSitePersonelId
        )
        : IRequestByServiceResult<bool>;
}
