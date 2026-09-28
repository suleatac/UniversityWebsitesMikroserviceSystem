using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.PageSectionDtos;

namespace Mikroservice.Site.Application.Features.PageSectionFeatures.GetPageSections
{
    /// <summary>
    /// Site+dil bazinda bolumleri (block agaciyla) getirir.
    /// PublishedOnly=true -> yalnizca yayindaki bolumler (web kullanimi).
    /// </summary>
    public record GetPageSectionsQuery(
        int SiteId,
        int DilId,
        bool PublishedOnly = false
    ) : IRequestByServiceResult<List<PageSectionDto>>;
}
