using Refit;

namespace Microservice.Admin.Clients.SeoCheckClients
{
    // Site API uzerindeki site-geneli SeoUrl kullanilabilirlik kontrol ucu.
    // Dondurulan bool: true => SeoUrl bosutta, false => baaska bir içerik tarafindan alinmis.
    public interface ISeoCheckClientServices
    {
        // Refit, ASP.NET Core yol kisitlamalarini ({siteId:int} gibi) desteklemez.
        // Kisitlama Refit tarafinda "siteId:int" adinda bir parametre gibi okundugu icin
        // "no method parameter matches" hatasi verir. Bu yuzden kisitlamalar olmadan yazilmali.
        [Get("/api/v1/icerikler/seo-available/{siteId}/{pageTypeId}/{seoUrl}")]
        Task<ApiResponse<bool>> IsSeoUrlAvailableAsync(int siteId, int pageTypeId, string seoUrl,
            [AliasAs("excludeIcerikId")] int? excludeIcerikId = null);
    }
}
