using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.DeleteBandLogo
{
    public class DeleteBandLogoCommandHandler(
          IBandLogoRepository bandLogoRepository,
          IUnitOfWork unitOfWork,
          IRedisCacheService redisCache
        )
        : IRequestHandler<DeleteBandLogoCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteBandLogoCommand request, CancellationToken cancellationToken)
        {
            var bandLogo = await bandLogoRepository.GetByIdAsync(request.Id);
            if (bandLogo == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

            bandLogoRepository.Delete(bandLogo);

            await unitOfWork.SaveChangesAsync(cancellationToken);

 
            //Cache temizleme işlemi.
            var cacheKey = $"bandlogos:list:{bandLogo.SiteId}:*";
            await redisCache.RemoveByPatternAsync(cacheKey, cancellationToken);


            return ServiceResult.Success();
        }
    }
}
