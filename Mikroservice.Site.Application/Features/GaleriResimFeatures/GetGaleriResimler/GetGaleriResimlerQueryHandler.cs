using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.GaleriResimDtos;

namespace Mikroservice.Site.Application.Features.GaleriResimFeatures.GetGaleriResimler
{
    public class GetGaleriResimlerQueryHandler(
        IGaleriResimRepository galeriResimRepository,
        IRedisCacheService redis,
        ILogger<GetGaleriResimlerQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetGaleriResimlerQuery, ServiceResult<List<GaleriResimDto>>>
    {
        public async Task<ServiceResult<List<GaleriResimDto>>> Handle(
            GetGaleriResimlerQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"galeriresim:list:{request.SiteId}:{request.DilId}";

            var cached = await redis.GetListAsync<GaleriResimDto>(cacheKey, cancellationToken);

            if (cached is not null)
            {
                logger.LogInformation("GaleriResim cache'den alındı");
                return ServiceResult<List<GaleriResimDto>>.SuccessAsOK(cached);
            }

            // Yoksa veritabanından çek
            var data = await galeriResimRepository.GetBySiteAndLanguageAsync(request.SiteId, request.DilId, cancellationToken);

            logger.LogInformation(
                "GaleriResim verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);

            var mappedData = mapper.Map<List<GaleriResimDto>>(data);

            await redis.SetListAsync(cacheKey, mappedData, TimeSpan.FromHours(12), cancellationToken);

            return ServiceResult<List<GaleriResimDto>>.SuccessAsOK(mappedData);
        }
    }
}
