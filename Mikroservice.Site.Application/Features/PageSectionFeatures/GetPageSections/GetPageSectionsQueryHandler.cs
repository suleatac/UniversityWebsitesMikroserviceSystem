using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.PageSectionDtos;

namespace Mikroservice.Site.Application.Features.PageSectionFeatures.GetPageSections
{
    public class GetPageSectionsQueryHandler(
        IPageSectionRepository pageSectionRepository,
        IRedisCacheService redisCacheService,
        IMapper mapper,
        ILogger<GetPageSectionsQueryHandler> logger
    ) : IRequestHandler<GetPageSectionsQuery, ServiceResult<List<PageSectionDto>>>
    {
        public async Task<ServiceResult<List<PageSectionDto>>> Handle(
            GetPageSectionsQuery request,
            CancellationToken cancellationToken)
        {
            // published/ unpublished listeleri ayri cache'lenir (web yalnizca published ceker).
            var flag = request.PublishedOnly ? "pub" : "all";
            var cacheKey = $"page-sections:list:{request.SiteId}:{request.DilId}:{flag}";

            var cached = await redisCacheService.GetListAsync<PageSectionDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation(
                    "PageSection verisi cacheden alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                    request.SiteId,
                    request.DilId,
                    cached.Count);

                return ServiceResult<List<PageSectionDto>>.SuccessAsOK(cached);
            }

            var data = await pageSectionRepository.GetPageSectionsAsync(
                request.SiteId,
                request.DilId,
                request.PublishedOnly,
                cancellationToken);

            var dto = mapper.Map<List<PageSectionDto>>(data);

            await redisCacheService.SetListAsync(cacheKey, dto, TimeSpan.FromHours(24), cancellationToken);

            logger.LogInformation(
                "PageSection verisi veritabanından alındı. SiteId:{siteId}, DilId:{dilId}, Count:{count}",
                request.SiteId,
                request.DilId,
                data.Count);

            return ServiceResult<List<PageSectionDto>>.SuccessAsOK(dto);
        }
    }
}
