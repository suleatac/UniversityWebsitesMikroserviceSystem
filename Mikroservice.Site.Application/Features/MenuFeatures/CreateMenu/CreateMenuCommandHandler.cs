using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.MenuEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.MenuFeatures.CreateMenu
{
    public class CreateMenuCommandHandler(
        IMenuRepository menuRepository,
        IUnitOfWork unitOfWork,
        IPublishEndpoint publishEndpoint
    ) : IRequestHandler<CreateMenuCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(CreateMenuCommand request, CancellationToken cancellationToken)
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

                OlusturulmaTarihi = DateTime.UtcNow,
                IsDeleted = false
            };

            await menuRepository.AddAsync(menu);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Event (cache temizleme için)
            await publishEndpoint.Publish(new MenuChangedEvent(menu.SiteId, menu.DilId),cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
