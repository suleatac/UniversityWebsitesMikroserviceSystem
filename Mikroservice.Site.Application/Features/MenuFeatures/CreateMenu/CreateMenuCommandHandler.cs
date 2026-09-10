using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;
using Mikroservice.Site.Domain.Enums;

namespace Mikroservice.Site.Application.Features.MenuFeatures.CreateMenu
{
    public class CreateMenuCommandHandler(
        IMenuRepository menuRepository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<CreateMenuCommand, ServiceResult<CreateMenuResponse>>
    {
        public async Task<ServiceResult<CreateMenuResponse>> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
        {
         

            var menu = new Menu {
                PageTypeId = request.PageTypeId,
                SiteId = request.SiteId,
                DilId = request.DilId,
                HedefId = request.HedefId,

                Baslik = request.Baslik,
                Link = request.Link,
                IcerikMetni = request.IcerikMetni,

                Sira = request.Sira,
                MegaMenu = request.MegaMenu,
                Location = (MenuLocation)request.Location,
                IsVisible = request.IsVisible,
                ParentId = request.ParentId,

                SeoUrl = request.SeoUrl,
                SeoTitle = request.SeoTitle,
                SeoDescription = request.SeoDescription,

                EklemeTarihi = DateTime.Now,
                IsDeleted = false
            };

            await menuRepository.AddAsync(menu);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 cache temizleme işlemi
           

            await redisCache.RemoveByPatternAsync($"menus:list:{menu.SiteId}:{menu.DilId}:*", cancellationToken);

            var response = new CreateMenuResponse(menu.Id);
            return ServiceResult<CreateMenuResponse>
            .SuccessAsCreated(response, $"/api/v1/menus/{menu.Id}");
        }
    }
}
