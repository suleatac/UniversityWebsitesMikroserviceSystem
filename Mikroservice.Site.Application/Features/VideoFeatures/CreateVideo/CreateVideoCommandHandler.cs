using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.VideoFeatures.CreateVideo
{
    public class CreateVideoCommandHandler(
     IVideoRepository repository,
     IUnitOfWork unitOfWork,
     IRedisCacheService redisCache
 ) : IRequestHandler<CreateVideoCommand, ServiceResult<CreateVideoResponse>>
    {
        public async Task<ServiceResult<CreateVideoResponse>> Handle(CreateVideoCommand request, CancellationToken cancellationToken)
        {
            var entity = new Video
            {
                SiteId = request.SiteId,
                DilId = request.DilId,
                HedefId = request.HedefId,

                Baslik = request.Baslik,
                KisaAciklama = request.KisaAciklama,
                IcerikMetni = request.IcerikMetni,

                Link = request.Link,
                ResimUrl = request.ResimUrl,

                YayimTarihi = request.YayimTarihi,
                EklemeTarihi = request.EklemeTarihi,

                BaslamaTarihi = request.BaslamaTarihi,
                BitisTarihi = request.BitisTarihi,

                SeoUrl = request.SeoUrl,
                SeoTitle = request.SeoTitle,
                SeoDescription = request.SeoDescription,

                VideoUrl = request.VideoUrl,

                GosterimSayisi = 0,
                IsDeleted = false
            };

            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

   

            var key = $"videos:list:{request.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            var response = new CreateVideoResponse(entity.Id);
            return ServiceResult<CreateVideoResponse>
            .SuccessAsCreated(response, $"/api/v1/videos/{entity.Id}");
        }
    }
}
