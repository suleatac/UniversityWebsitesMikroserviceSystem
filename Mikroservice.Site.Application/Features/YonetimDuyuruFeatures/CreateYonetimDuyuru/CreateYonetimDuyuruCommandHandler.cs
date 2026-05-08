using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Domain.Entities;
using Mikroservice.Site.Application.Features.SiteFeatures.CreateSite;
using Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.CreateYonetimDuyuru;
using Mikroservice.Site.Domain.Entities;

namespace Microservice.Site.Application.Features.YonetimDuyuruFeatures.CreateYonetimDuyuru
{
    public class CreateYonetimDuyuruCommandHandler
        (
          IYonetimDuyuruRepository yonetimDuyuruRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCacheService
        )
        : IRequestHandler<CreateYonetimDuyuruCommand, ServiceResult<CreateYonetimDuyuruResponse>>
    {
        public async Task<ServiceResult<CreateYonetimDuyuruResponse>> Handle(CreateYonetimDuyuruCommand request, CancellationToken cancellationToken)
        {
            var newYonetimDuyuru = new YonetimDuyuru {
                Baslik = request.Baslik,
                Icerik = request.Icerik,
                EklenmeTarihi = request.EklenmeTarihi
            };
            await yonetimDuyuruRepository.AddAsync(newYonetimDuyuru);
            await unitOfWork.SaveChangesAsync();

            var cacheKey = "yonetimduyuru:*";
            await redisCacheService.RemoveByPatternAsync(cacheKey, cancellationToken);

            var response = new CreateYonetimDuyuruResponse(newYonetimDuyuru.Id);
            return ServiceResult<CreateYonetimDuyuruResponse>
            .SuccessAsCreated(response, $"/api/v1/yonetimDuyuru/{newYonetimDuyuru.Id}");
        }
    }
}
