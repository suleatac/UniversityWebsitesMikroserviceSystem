using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

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
                SiteId = request.SiteId,
                DilId = request.DilId,
                HedefId = request.HedefId,

                Ad = request.Ad,
                Link = request.Link,
                IconUrl = request.IconUrl,
                Icerik = request.Icerik,

                Sira = request.Sira,
                MegaMenu = request.MegaMenu,
                ParentId = request.ParentId,

                OlusturulmaTarihi = DateTime.Now,
                IsDeleted = false
            };

            await menuRepository.AddAsync(menu);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 cache temizleme işlemi
           

            var key = $"menus:list:{menu.SiteId}:{menu.DilId}";
            await redisCache.RemoveAsync(key, cancellationToken);

            var response = new CreateMenuResponse(menu.Id);
            return ServiceResult<CreateMenuResponse>
            .SuccessAsCreated(response, $"/api/v1/menus/{menu.Id}");
        }
    }
}
