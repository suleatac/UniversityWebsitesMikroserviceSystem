using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.MenuEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Mikroservice.Site.Application.Features.MenuFeatures.UpdateMenu
{
    public class UpdateMenuCommandHandler(
     IMenuRepository menuRepository,
     IUnitOfWork unitOfWork,
     IPublishEndpoint publishEndpoint
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
            menu.Ad = request.Ad;
            menu.Link = request.Link;
            menu.IconUrl = request.IconUrl;
            menu.Icerik = request.Icerik;

            menu.Sira = request.Sira;
            menu.MegaMenu = request.MegaMenu;

            menu.ParentId = request.ParentId;

            // 🔹 (Opsiyonel) hedef değiştirilebilir
            menu.HedefId = request.HedefId;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Event (cache invalidation)
            await publishEndpoint.Publish(
                new MenuChangedEvent(menu.SiteId, menu.DilId),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
