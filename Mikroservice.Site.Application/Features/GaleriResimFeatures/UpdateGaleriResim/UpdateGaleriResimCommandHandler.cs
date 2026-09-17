using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.GaleriResimFeatures.UpdateGaleriResim
{
    public class UpdateGaleriResimCommandHandler(
        IGaleriResimRepository repository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<UpdateGaleriResimCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateGaleriResimCommand request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);

            if (entity == null || entity.IsDeleted)
                return ServiceResult.ErrorAsNotFound();

            entity.Baslik = request.Baslik;
            entity.KisaAciklama = request.KisaAciklama;
            entity.IcerikMetni = request.IcerikMetni;

            entity.Link = request.Link;
            entity.ResimUrl = request.ResimUrl;

            entity.Kategori = request.Kategori;
            entity.Sira = request.Sira;

            entity.YayimTarihi = request.YayimTarihi;

            entity.BaslamaTarihi = request.BaslamaTarihi;
            entity.BitisTarihi = request.BitisTarihi;

            entity.SeoUrl = request.SeoUrl;
            entity.SeoTitle = request.SeoTitle;
            entity.SeoDescription = request.SeoDescription;

            entity.PageTypeId = request.PageTypeId;

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var key = $"galeriresim:list:{request.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
