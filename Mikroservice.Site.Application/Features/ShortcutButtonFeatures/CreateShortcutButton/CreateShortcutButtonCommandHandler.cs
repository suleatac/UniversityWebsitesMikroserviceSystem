using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.ShortcutButtonFeatures.CreateShortcutButton
{
    public class CreateShortcutButtonCommandHandler(
        IShortcutButtonRepository shortcutButtonRepository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<CreateShortcutButtonCommand, ServiceResult<CreateShortcutButtonResponse>>
    {
        public async Task<ServiceResult<CreateShortcutButtonResponse>> Handle(CreateShortcutButtonCommand request, CancellationToken cancellationToken)
        {
            var shortcutButton = new ShortcutButton {
                SiteId = request.SiteId,
                DilId = request.DilId,
                HedefId = request.HedefId,

                Ad = request.Ad,
                KisaAciklama = request.KisaAciklama,
                Link = request.Link,
                IconUrl = request.IconUrl,
                ImageUrl = request.ImageUrl,
                IsIconImage = request.IsIconImage,

                Sira = request.Sira,

                OlusturulmaTarihi = DateTime.Now,
                IsDeleted = false
            };

            await shortcutButtonRepository.AddAsync(shortcutButton);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            // 🔥 cache temizleme işlemi
            var key = $"shortcutbuttons:list:{shortcutButton.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            var response = new CreateShortcutButtonResponse(shortcutButton.Id);
            return ServiceResult<CreateShortcutButtonResponse>
            .SuccessAsCreated(response, $"/api/v1/shortcutButtons/{shortcutButton.Id}");
        }
    }
}
