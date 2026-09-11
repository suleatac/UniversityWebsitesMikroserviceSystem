using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.ShortcutButtonFeatures.UpdateShortcutButton
{
    public class UpdateShortcutButtonCommandHandler(
          IShortcutButtonRepository shortcutButtonRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        ) : IRequestHandler<UpdateShortcutButtonCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateShortcutButtonCommand request, CancellationToken cancellationToken)
        {
            var shortcutButton = await shortcutButtonRepository.GetByIdAsync(request.Id);
            if (shortcutButton == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            shortcutButton.Ad = request.Ad;
            shortcutButton.KisaAciklama = request.KisaAciklama;
            shortcutButton.Link = request.Link;
            shortcutButton.IconUrl = request.IconUrl;
            shortcutButton.ImageUrl = request.ImageUrl;
            shortcutButton.IsIconImage = request.IsIconImage;
            shortcutButton.Sira = request.Sira;
            shortcutButton.SiteId = request.SiteId;
            shortcutButton.DilId = request.DilId;
            shortcutButton.HedefId = request.HedefId;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var cacheKey = $"shortcutbuttons:list:{shortcutButton.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
