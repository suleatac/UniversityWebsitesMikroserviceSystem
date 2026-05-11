using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.EtkinlikFeatures.UpdateEtkinlik
{
    public class UpdateEtkinlikCommandHandler(
          IEtkinlikRepository etkinlikRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<UpdateEtkinlikCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateEtkinlikCommand request, CancellationToken cancellationToken)
        {
            var etkinlik = await etkinlikRepository.GetByIdAsync(request.Id);
            if (etkinlik == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            etkinlik.Baslik = request.Baslik;
            etkinlik.KisaAciklama = request.KisaAciklama;
            etkinlik.IcerikMetni = request.IcerikMetni;
            etkinlik.Link = request.Link;
            etkinlik.ResimUrl = request.ResimUrl;
            etkinlik.YayimTarihi = request.YayimTarihi;
            etkinlik.BaslamaTarihi = request.BaslamaTarihi;
            etkinlik.BitisTarihi = request.BitisTarihi;
            etkinlik.SeoUrl = request.SeoUrl;
            etkinlik.SeoTitle = request.SeoTitle;
            etkinlik.SeoDescription = request.SeoDescription;
            etkinlik.SiteId = request.SiteId;
            etkinlik.DilId = request.DilId;
            etkinlik.HedefId = request.HedefId;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var cacheKey = $"etkinliks:list:{etkinlik.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);

            return ServiceResult.SuccessAsNoContent();
        }
    }
}
