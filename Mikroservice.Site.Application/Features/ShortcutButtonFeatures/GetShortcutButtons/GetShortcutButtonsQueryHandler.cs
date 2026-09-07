using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.DuyuruDtos;
using Mikroservice.Site.Application.DTOs.ShortcutButtonDtos;


namespace Mikroservice.Site.Application.Features.ShortcutButtonFeatures.GetShortcutButtons
{
    public class GetShortcutButtonsQueryHandler(
          IShortcutButtonRepository shortcutButtonRepository,
          IRedisCacheService redisCacheService,
          IMapper mapper,
          ILogger<GetShortcutButtonsQueryHandler> logger
        ) : IRequestHandler<GetShortcutButtonsQuery, ServiceResult<List<ShortcutButtonDto>>>
    {
        public async Task<ServiceResult<List<ShortcutButtonDto>>> Handle(GetShortcutButtonsQuery request, CancellationToken cancellationToken)
        {
            // Önce cache'e bak
            var cacheKey = $"shortcutbuttons:list:{request.SiteId}:{request.DilId}";
            var cached = await redisCacheService.GetListAsync<ShortcutButtonDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                //Örnek Loglama
                logger.LogInformation(
                    "Shortcut button verisi cacheden alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                    request.SiteId,
                    request.DilId,
                    cached.Count);
                return ServiceResult<List<ShortcutButtonDto>>.SuccessAsOK(cached);
            }


            // Yoksa veritabanından çek
            var data = await shortcutButtonRepository.GetShortcutButtonsBySiteAndDilAsync(request.SiteId, request.DilId, cancellationToken);
            var dto = mapper.Map<List<ShortcutButtonDto>>(data);
            // Cache'e yaz
            await redisCacheService.SetListAsync(cacheKey, dto, TimeSpan.FromHours(24), cancellationToken);

            //Örnek Loglama
            logger.LogInformation(
                "Shortcut button verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);
            return ServiceResult<List<ShortcutButtonDto>>.SuccessAsOK(dto);
            
        }


    
    }
}
