using Microservice.Shared;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.IcerikDtos;

namespace Mikroservice.Site.Application.Features.IcerikFeatures.SearchIcerik
{
    /// <summary>
    /// Icerik tablosu (TPH) uzerinde Baslik, KisaAciklama ve IcerikMetni
    /// alanlarinda sayfali arama yapar.
    /// Tip: IcerikTip degeri (1=Haber,2=Duyuru,3=Bilgi,4=Etkinlik,5=Video).
    /// Bos gonderilirse Menu ve Banner haric tum icerik turlerinde aranir.
    /// </summary>
    public record SearchIcerikQuery(
        int SiteId,
        int DilId,
        string? Search = null,
        int? Tip = null,
        int Page = 1,
        int PageSize = 5
    ) : IRequestByServiceResult<PaginatedResult<IcerikSearchDto>>;
}
