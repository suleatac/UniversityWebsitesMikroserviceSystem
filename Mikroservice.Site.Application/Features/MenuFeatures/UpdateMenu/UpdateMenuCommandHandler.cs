using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.MenuEvents;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Mikroservice.Site.Application.Features.MenuFeatures.UpdateMenu
{
    public class UpdateMenuCommandHandler(
     IMenuRepository menuRepository,
     IUnitOfWork unitOfWork,
    IRedisCacheService redisCache
 ) : IRequestHandler<UpdateMenuCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateMenuCommand request, CancellationToken cancellationToken)
        {
            // 🔹 Menü var mı?
            var menu = await menuRepository.GetByIdAsync(request.Id);
            if (menu == null || menu.IsDeleted)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            // 🔥 Parent kontrolü
            if (request.ParentId.HasValue)
            {
                // ❗ Kendini parent yapamaz
                if (request.ParentId == request.Id)
                {
                    return ServiceResult.Error("Bir menü kendisini üst menü yapamaz.",HttpStatusCode.BadRequest);
                }

            }

            // 🔥 Güncelleme
            menu.Baslik = request.Baslik;
            menu.Link = request.Link;
            menu.IcerikMetni = request.IcerikMetni;

            menu.Sira = request.Sira;
            menu.MegaMenu = request.MegaMenu;
            menu.Location = (MenuLocation)request.Location;
            menu.IsVisible = request.IsVisible;

            menu.ParentId = request.ParentId;

            menu.SeoUrl = request.SeoUrl;
            menu.SeoTitle = request.SeoTitle;
            menu.SeoDescription = request.SeoDescription;

            // 🔹 (Opsiyonel) hedef değiştirilebilir
            menu.HedefId = request.HedefId;
            menu.PageTypeId = request.PageTypeId;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Event (cache invalidation)
            await redisCache.RemoveByPatternAsync($"menus:list:{menu.SiteId}:{menu.DilId}:*", cancellationToken);

            return ServiceResult.Success();
        }
    }
}
