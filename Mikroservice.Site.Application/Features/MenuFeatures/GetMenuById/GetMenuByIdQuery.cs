using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.MenuDtos;

namespace Mikroservice.Site.Application.Features.MenuFeatures.GetMenuById
{
    public record GetMenuByIdQuery(int Id) : IRequestByServiceResult<MenuDetailDto>;
}