using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.VideoFeatures.UpdateVideo
{
    public class UpdateVideoCommandHandler(
       IVideoRepository repository,
       IUnitOfWork unitOfWork,
       IRedisCacheService redisCache
   ) : IRequestHandler<UpdateVideoCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateVideoCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            entity.Baslik = request.Baslik;
            entity.KisaAciklama = request.KisaAciklama;
            entity.IcerikMetni = request.IcerikMetni;

            entity.Link = request.Link;
            entity.ResimUrl = request.ResimUrl;

            entity.YayimTarihi = request.YayimTarihi;

            entity.BaslamaTarihi = request.BaslamaTarihi;
            entity.BitisTarihi = request.BitisTarihi;

            entity.SeoUrl = request.SeoUrl;
            entity.SeoTitle = request.SeoTitle;
            entity.SeoDescription = request.SeoDescription;

            entity.VideoUrl = request.VideoUrl;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var key = $"videos:list:{request.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
