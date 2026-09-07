using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.ShortcutButtonFeatures.DeleteShortcutButton
{
    public class DeleteShortcutButtonCommandHandler(
         IShortcutButtonRepository shortcutButtonRepository,
         IUnitOfWork unitOfWork,
         IRedisCacheService redisCache
     ) : IRequestHandler<DeleteShortcutButtonCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteShortcutButtonCommand request, CancellationToken cancellationToken)
        {
            var shortcutButton = await shortcutButtonRepository.GetByIdAsync(request.Id);

            if (shortcutButton == null || shortcutButton.IsDeleted)
            {
                return ServiceResult.ErrorAsNotFound();
            }
            shortcutButton.IsDeleted = true;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 Cache invalidation
            var key = $"shortcutbuttons:list:{shortcutButton.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
