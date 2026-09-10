using Microservice.Shared;

namespace Mikroservice.Site.Application.Features.IcerikFeatures.IsSeoUrlAvailable
{
    // SeoUrl'un site genelinde (tüm içerik türleri ve diller dahil, Icerik tablosu + SSS tablosu)
    // kullanilabilir olup olmadigini kontrol eder.
    // excludeIcerikId / excludeSikcaSorulanSoruId: guncleme sirasinda kayd'in kendisinin carpisma sayilmamasi icin verilir.
    public record IsSeoUrlAvailableQuery(
        int SiteId, 
        string SeoUrl,
        int PageTypeId,
        int? ExcludeIcerikId
        )
        : IRequestByServiceResult<bool>;
}
