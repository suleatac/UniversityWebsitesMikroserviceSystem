using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.MenuDtos;

namespace Mikroservice.Site.Application.Features.MenuFeatures.GetMenuBySeoUrl
{
 
    public record GetMenuBySeoUrlQuery(int SiteId, int DilId, string SeoUrl) : IRequestByServiceResult<MenuDetailDto>;
}
