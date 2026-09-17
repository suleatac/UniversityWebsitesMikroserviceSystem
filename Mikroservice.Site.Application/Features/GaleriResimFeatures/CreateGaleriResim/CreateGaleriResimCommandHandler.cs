using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.GaleriResimFeatures.CreateGaleriResim
{
    public class CreateGaleriResimCommandHandler(
        IGaleriResimRepository repository,
        IUnitOfWork unitOfWork,
        IRedisCacheService redisCache
    ) : IRequestHandler<CreateGaleriResimCommand, ServiceResult<CreateGaleriResimResponse>>
    {
        public async Task<ServiceResult<CreateGaleriResimResponse>> Handle(CreateGaleriResimCommand request, CancellationToken cancellationToken)
        {
            var entity = new GaleriResim
            {
                PageTypeId = request.PageTypeId,
                SiteId = request.SiteId,
                DilId = request.DilId,
                HedefId = request.HedefId,

                Baslik = request.Baslik,
                KisaAciklama = request.KisaAciklama,
                IcerikMetni = request.IcerikMetni,

                Link = request.Link,
                ResimUrl = request.ResimUrl,

                Kategori = request.Kategori,
                Sira = request.Sira,

                YayimTarihi = request.YayimTarihi,
                EklemeTarihi = request.EklemeTarihi,

                BaslamaTarihi = request.BaslamaTarihi,
                BitisTarihi = request.BitisTarihi,

                SeoUrl = request.SeoUrl,
                SeoTitle = request.SeoTitle,
                SeoDescription = request.SeoDescription,

                GosterimSayisi = 0,
                IsDeleted = false
            };

            await repository.AddAsync(entity);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var key = $"galeriresim:list:{request.SiteId}:*";
            await redisCache.RemoveByPatternAsync(key, cancellationToken);

            var response = new CreateGaleriResimResponse(entity.Id);
            return ServiceResult<CreateGaleriResimResponse>
                .SuccessAsCreated(response, $"/api/v1/galeri-resimler/{entity.Id}");
        }
    }
}
