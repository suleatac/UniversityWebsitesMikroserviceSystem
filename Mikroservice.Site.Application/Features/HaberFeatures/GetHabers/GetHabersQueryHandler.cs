using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.HaberDtos;
using Mikroservice.Site.Application.DTOs.TemplateDtos;

namespace Mikroservice.Site.Application.Features.HaberFeatures.GetHabers
{
    public class GetHabersQueryHandler(
          IHaberRepository haberRepository,
          IRedisCacheService redisCacheService,
          ILogger<GetHabersQueryHandler> logger,
          IMapper mapper
        )
        : IRequestHandler<GetHabersQuery, ServiceResult<List<HaberDto>>>
    {
        public async Task<ServiceResult<List<HaberDto>>> Handle(GetHabersQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"habers:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<HaberDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                "Haber cache'den alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                cached.Count);
                return ServiceResult<List<HaberDto>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = await haberRepository.GetAll().Where(b => b.SiteId == request.SiteId && b.DilId == request.DilId).ToListAsync(cancellationToken);

         
            //Örnek Loglama
            logger.LogInformation(
                "Haber verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);

            var mappedData = mapper.Map<List<HaberDto>>(data);

            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, mappedData, TimeSpan.FromHours(24), cancellationToken);



            return ServiceResult<List<HaberDto>>.SuccessAsOK(mappedData);
       
        }
    }
}
