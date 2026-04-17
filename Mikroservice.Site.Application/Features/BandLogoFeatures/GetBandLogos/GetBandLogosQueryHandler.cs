using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.GetBandLogos
{
    public class GetBandLogosQueryHandler(
          IBandLogoRepository bandLogoRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetBandLogosQueryHandler> logger
        )
        : IRequestHandler<GetBandLogosQuery, ServiceResult<List<BandLogo>>>
    {
        public async Task<ServiceResult<List<BandLogo>>> Handle(GetBandLogosQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"bandlogos:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<BandLogo>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                    "Band logoları verisi cacheden alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                    request.SiteId,
                    request.DilId,
                    cached.Count);
                return ServiceResult<List<BandLogo>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = bandLogoRepository.GetAll().Where(b => b.SiteId == request.SiteId && b.DilId == request.DilId).ToList();

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, data, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation(
                "Band logoları verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);
            return ServiceResult<List<BandLogo>>.SuccessAsOK(data);
        }
    }
}
