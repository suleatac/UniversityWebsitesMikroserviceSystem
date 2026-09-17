using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Features.Common;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.UpdateDuyuru
{
    public class UpdateDuyuruCommandHandler(
          IDuyuruRepository duyuruRepository,
          IIcerikDosyaRepository icerikDosyaRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<UpdateDuyuruCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateDuyuruCommand request, CancellationToken cancellationToken)
        {
            var duyuru = await duyuruRepository.GetByIdAsync(request.Id);
            if (duyuru == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            duyuru.Baslik = request.Baslik;
            duyuru.KisaAciklama = request.KisaAciklama;
            duyuru.IcerikMetni = request.IcerikMetni;
            duyuru.Link = request.Link;
            duyuru.ResimUrl = request.ResimUrl;
            duyuru.YayimTarihi = request.YayimTarihi;
            duyuru.BaslamaTarihi = request.BaslamaTarihi;
            duyuru.BitisTarihi = request.BitisTarihi;
            duyuru.SeoUrl = request.SeoUrl;
            duyuru.SeoTitle = request.SeoTitle;
            duyuru.SeoDescription = request.SeoDescription;
            duyuru.SiteId = request.SiteId;
            duyuru.DilId = request.DilId;
            duyuru.HedefId = request.HedefId;
            duyuru.PageTypeId = request.PageTypeId;

            // Ek dosyalari senkronize et: mevcutlar TRACKED okunur (guncelleme/silme izlenir),
            // yeni dosyalar ayrica AddAsync edilir.
            var mevcutDosyalar = await icerikDosyaRepository.GetTrackedByIcerikIdsAsync(new[] { duyuru.Id }, cancellationToken);
            var yeniDosyalar = IcerikDosyaSync.Apply(mevcutDosyalar, request.Dosyalar, duyuru.Id);

            foreach (var yeni in yeniDosyalar)
                await icerikDosyaRepository.AddAsync(yeni);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var cacheKey = $"duyuru:list:{duyuru.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
