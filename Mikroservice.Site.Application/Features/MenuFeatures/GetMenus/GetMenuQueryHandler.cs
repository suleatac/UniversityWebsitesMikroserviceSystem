using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.MenuDtos;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Application.Features.MenuFeatures.GetMenus
{
    public class GetMenuQueryHandler(
          IMenuRepository menuRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetMenuQueryHandler> logger
        )
        : IRequestHandler<GetMenuQuery, ServiceResult<List<MenuDto>>>
    {
        public async Task<ServiceResult<List<MenuDto>>> Handle(GetMenuQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"menus:list:{request.SiteId}:{request.DilId}:{request.Location?.ToString() ?? "all"}";

            // ✔ Cache kontrol
            var cached = await redisCacheService.GetListAsync<MenuDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation(
                    "Menu cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                    request.SiteId,
                    request.DilId,
                    cached.Count);

                return ServiceResult<List<MenuDto>>.SuccessAsOK(cached);
            }

            // ✔ DB'den flat veri çek
            var query = menuRepository.GetAll()
                .Where(x =>
                    x.SiteId == request.SiteId &&
                    x.DilId == request.DilId &&
                    x.IsVisible);

            if (request.Location.HasValue)
            {
                var location = (MenuLocation)request.Location.Value;
                query = query.Where(x => x.Location == location);
            }

            var data = query
                .OrderBy(x => x.Sira)
                .ToList();

            // ✔ Tree oluştur
            var tree = BuildTree(data);

            // ✔ Cache'e yaz
            await redisCacheService.SetListAsync(
                cacheKey,
                tree,
                TimeSpan.FromHours(6),
                cancellationToken);

            logger.LogInformation(
                "Menu DB'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                tree.Count);

            return ServiceResult<List<MenuDto>>.SuccessAsOK(tree);
        }

        private List<MenuDto> BuildTree(List<Menu> list, int? parentId = null)
        {
            return list
                .Where(x => x.ParentId == parentId)
                .OrderBy(x => x.Sira)
                .Select(x => new MenuDto {
                    Id = x.Id,
                    SiteId = x.SiteId,
                    DilId = x.DilId,
                    HedefId = x.HedefId,
                    Baslik = x.Baslik,
                    Link = x.Link,
                    IcerikMetni = x.IcerikMetni,
                    Sira = x.Sira,
                    MegaMenu = x.MegaMenu,
                    Location = (int)x.Location,
                    IsVisible = x.IsVisible,
                    ParentId = x.ParentId,
                    Children = BuildTree(list, x.Id)
                })
                .ToList();
        }
    }
}
