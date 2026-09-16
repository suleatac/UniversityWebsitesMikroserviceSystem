using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs.SitePersonelDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.SitePersonelFeatures.GetSitePersonelById
{
    public class GetSitePersonelByIdQueryHandler(
        ISitePersonelRepository sitePersonelRepository,
        IRedisCacheService redisCacheService,
        ILogger<GetSitePersonelByIdQueryHandler> logger,
        IMapper mapper
      )
      : IRequestHandler<GetSitePersonelByIdQuery, ServiceResult<SitePersonelDetailDto>>
    {
        public async Task<ServiceResult<SitePersonelDetailDto>> Handle(GetSitePersonelByIdQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = $"site-personel:{request.Id}";

            var cached = await redisCacheService.GetAsync<SitePersonelDetailDto>(cacheKey, cancellationToken);
            if (cached is not null)
            {
                logger.LogInformation("SitePersonel cache'den alındı. Id: {Id}", request.Id);
                return ServiceResult<SitePersonelDetailDto>.SuccessAsOK(cached);
            }

            var entity = await sitePersonelRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("SitePersonel bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<SitePersonelDetailDto>.Error("SitePersonel bulunamadı", HttpStatusCode.NotFound);
            }
            // ✔ map
            var dto = mapper.Map<SitePersonelDetailDto>(entity);

            await redisCacheService.SetAsync(cacheKey, dto, TimeSpan.FromHours(24), cancellationToken);

            logger.LogInformation("SitePersonel DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<SitePersonelDetailDto>.SuccessAsOK(dto);
        }
    }
}