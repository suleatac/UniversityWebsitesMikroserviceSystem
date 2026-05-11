using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Features.SiteFeatures.CreateSite;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.CreateBandLogo
{
    public class CreateBandLogoCommandHandler(
          IBandLogoRepository bandLogoRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<CreateBandLogoCommand, ServiceResult<CreateBandLogoResponse>>
    {
        public async Task<ServiceResult<CreateBandLogoResponse>> Handle(CreateBandLogoCommand request, CancellationToken cancellationToken)
        {
            var newBandLogo = new BandLogo {
                Ad = request.Ad,
                ImgUrl = request.ImgUrl,
                Link = request.Link,
                SiteId = request.SiteId,
                DilId = request.DilId
            };
            await bandLogoRepository.AddAsync(newBandLogo);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var cacheKey = $"bandlogos:list:{newBandLogo.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);

            var response = new CreateBandLogoResponse(newBandLogo.Id);
            return ServiceResult<CreateBandLogoResponse>
            .SuccessAsCreated(response, $"/api/v1/bandlogos/{newBandLogo.Id}");
        }
    }
}
