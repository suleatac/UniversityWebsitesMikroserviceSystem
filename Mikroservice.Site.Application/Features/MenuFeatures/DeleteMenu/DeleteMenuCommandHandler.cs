using MassTransit;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RabbitMqMasstransitServiceItems.Events.MenuEvents;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Mikroservice.Site.Application.Features.MenuFeatures.DeleteMenu
{
    public class DeleteMenuCommandHandler(
         IMenuRepository menuRepository,
         IUnitOfWork unitOfWork,
         IPublishEndpoint publishEndpoint
     ) : IRequestHandler<DeleteMenuCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteMenuCommand request, CancellationToken cancellationToken)
        {
            var menu = await menuRepository.GetByIdAsync(request.Id);

            if (menu == null || menu.IsDeleted)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            // 🔥 ALT MENÜLERİ DE SİL (recursive soft delete)
            await SoftDeleteRecursive(menu.Id);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache invalidation
            await publishEndpoint.Publish(
                new MenuChangedEvent(menu.SiteId, menu.DilId),
                cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }

        private async Task SoftDeleteRecursive(int menuId)
        {
            var children = await menuRepository.Where(x => x.ParentId == menuId).ToListAsync();

            foreach (var child in children)
            {
                await SoftDeleteRecursive(child.Id);
            }

            var entity = await menuRepository.GetByIdAsync(menuId);
            if (entity != null)
            {
                entity.IsDeleted = true;
            }
        }
    }
}
