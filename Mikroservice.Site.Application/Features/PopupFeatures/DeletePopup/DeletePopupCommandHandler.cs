using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.PopupFeatures.DeletePopup
{
    public class DeletePopupCommandHandler(
          IPopupRepository popupRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<DeletePopupCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeletePopupCommand request, CancellationToken cancellationToken)
        {
            var popup = await popupRepository.GetByIdAsync(request.Id);
            if (popup == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            popup.IsDeleted = true;
            popupRepository.Update(popup);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var key = $"popup:list:{popup.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
