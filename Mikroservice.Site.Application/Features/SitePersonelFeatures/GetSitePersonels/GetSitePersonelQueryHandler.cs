using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.SitePersonelDtos;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.GetSitePersonels
{
    public class GetSitePersonelQueryHandler(
       ISitePersonelRepository repository,
       IRedisCacheService redis,
       ILogger<GetSitePersonelQueryHandler> logger,
       IMapper mapper
   ) : IRequestHandler<GetSitePersonelQuery, ServiceResult<List<SitePersonelDto>>>
    {
        public async Task<ServiceResult<List<SitePersonelDto>>> Handle(
            GetSitePersonelQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"sitepersonel:list:{request.SiteId}";

            var cached = await redis.GetListAsync<SitePersonelDto>(cacheKey, cancellationToken);

            if (cached is not null)
            {
                logger.LogInformation("SitePersonel cache'den alındı. SiteId:{siteId}", request.SiteId);
                return ServiceResult<List<SitePersonelDto>>.SuccessAsOK(cached);
            }

            var data = await repository.GetAllWithPersonelTipAndUnvanAsync(request.SiteId, cancellationToken);
            var mappedData = mapper.Map<List<SitePersonelDto>>(data);


            await redis.SetListAsync(cacheKey, mappedData, TimeSpan.FromHours(12), cancellationToken);

            return ServiceResult<List<SitePersonelDto>>.SuccessAsOK(mappedData);
        }
    }
}
