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

            // ✔ DB'den flat veri çek (PageType eager loading ile repository metodu üzerinden)
            MenuLocation? location = request.Location.HasValue
                ? (MenuLocation)request.Location.Value
                : null;

            var data = await menuRepository.GetMenusWithPageTypeAsync(
                request.SiteId,
                request.DilId,
                location,
                cancellationToken);

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
                    PageTypeId=x.PageTypeId,
                    PageType = x.PageType is null ? null : new MenuPageTypeDto
                    {
                        Id = x.PageType.Id,
                        PageTypeKind = x.PageType.PageTypeKind,
                        Name = x.PageType.Name,
                        Slug = x.PageType.Slug,
                        TemplateId = x.PageType.TemplateId,
                        ViewName = x.PageType.ViewName,
                        IsHomePage = x.PageType.IsHomePage
                    },
                    SeoUrl=x.SeoUrl,
                    SeoTitle=x.SeoTitle,
                    SeoDescription=x.SeoDescription,
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
