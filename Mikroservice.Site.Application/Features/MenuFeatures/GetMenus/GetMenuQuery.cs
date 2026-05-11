using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.MenuDtos;

namespace Mikroservice.Site.Application.Features.MenuFeatures.GetMenus
{
    public record GetMenuQuery(int SiteId, int DilId) : IRequestByServiceResult<List<MenuDto>>;
}
