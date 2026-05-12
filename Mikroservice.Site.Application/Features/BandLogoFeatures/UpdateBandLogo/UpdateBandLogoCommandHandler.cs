using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.UpdateBandLogo
{
    public class UpdateBandLogoCommandHandler(
          IBandLogoRepository bandLogoRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<UpdateBandLogoCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdateBandLogoCommand request, CancellationToken cancellationToken)
        {
            var bandLogo = await bandLogoRepository.GetByIdAsync(request.Id);
            if (bandLogo == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            bandLogo.Ad = request.Ad;
            bandLogo.ImgUrl = request.ImgUrl;
            bandLogo.Link = request.Link;
            bandLogo.SiteId = request.SiteId;
            bandLogo.DilId = request.DilId;
            await unitOfWork.SaveChangesAsync(cancellationToken);

            //Cache temizleme işlemi.
            var cacheKey = $"bandlogos:list:{bandLogo.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);

            return ServiceResult.Success();
        }
    }
}
