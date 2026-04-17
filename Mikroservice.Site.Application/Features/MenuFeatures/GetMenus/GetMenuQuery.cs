using Microservice.Shared;
using Mikroservice.Site.Application.Contracts.DTOs;

namespace Mikroservice.Site.Application.Features.MenuFeatures.GetMenus
{
    public record GetMenuQuery(int SiteId, int DilId) : IRequestByServiceResult<List<MenuDto>>;
}
