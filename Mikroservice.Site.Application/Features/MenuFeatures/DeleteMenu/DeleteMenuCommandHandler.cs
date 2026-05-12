using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Mikroservice.Site.Application.Features.MenuFeatures.DeleteMenu
{
    public class DeleteMenuCommandHandler(
         IMenuRepository menuRepository,
         IUnitOfWork unitOfWork,
         IRedisCacheService redisCache
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
            var key = $"menus:list:{menu.SiteId}:{menu.DilId}";
            await redisCache.RemoveAsync(key, cancellationToken);

            return ServiceResult.Success();
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
