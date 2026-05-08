using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.SiteDtos;
using Mikroservice.Site.Application.DTOs.YonetimDuyuru;
using Mikroservice.Site.Application.Features.SiteFeatures.GetPaginatedSite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.GetPaginatedYonetimDuyuru
{
    public class GetPaginatedYonetimDuyuruQueryHandler(
        IYonetimDuyuruRepository yonetimDuyuruRepository,
        IRedisCacheService redis,
        ILogger<GetPaginatedYonetimDuyuruQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetPaginatedYonetimDuyuruQuery, ServiceResult<PaginatedResult<YonetimDuyuruDto>>>
    {
        public async Task<ServiceResult<PaginatedResult<YonetimDuyuruDto>>> Handle(
            GetPaginatedYonetimDuyuruQuery request,
            CancellationToken cancellationToken)
        {
            // Query-specific cache key
            var cacheKey =
                $"yonetimduyuru:list:" +
                $"p:{request.Page}:" +
                $"ps:{request.PageSize}:" +
                $"s:{request.Search}:" +
                $"ob:{request.OrderBy}:" +
                $"od:{request.OrderDir}";

            // Cache kontrolü
            var cachedResult =
                await redis.GetAsync<PaginatedResult<YonetimDuyuruDto>>(cacheKey, cancellationToken);

            if (cachedResult is not null)
            {
                logger.LogInformation("YonetimDuyuru listesi cache'den getirildi");

                return ServiceResult<PaginatedResult<YonetimDuyuruDto>>
                    .SuccessAsOK(cachedResult);
            }

            // Base query
            IQueryable<YonetimDuyuru> query = yonetimDuyuruRepository.GetAll();

            // Search
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();

                query = query.Where(x =>
                    x.Baslik.ToLower().Contains(search) ||
                    x.Icerik.ToLower().Contains(search) );
            }

            // Ordering
            query = (request.OrderBy?.ToLower(), request.OrderDir?.ToLower()) switch {
                ("baslik", "asc") =>
                    query.OrderBy(x => x.Baslik),

                ("icerik", "desc") =>
                    query.OrderByDescending(x => x.Icerik),
                _ =>
                    query.OrderByDescending(x => x.Id)
            };

            // Total count
            var totalCount = await query.CountAsync(cancellationToken);

            // Pagination
            if (request.PageSize != -1)
            {
                query = query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize);
            }

            // DTO projection
            var data = await query
                .ProjectTo<YonetimDuyuruDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            // Result
            var result = new PaginatedResult<YonetimDuyuruDto> {
                Data = data,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            // Cache save (5 dk)
            await redis.SetAsync(
                cacheKey,
                result,
                TimeSpan.FromMinutes(5),
                cancellationToken);

            logger.LogInformation(
                "YonetimDuyuru listesi DB'den getirildi. TotalCount: {TotalCount}, Page: {Page}",
                totalCount,
                request.Page);

            return ServiceResult<PaginatedResult<YonetimDuyuruDto>>
                .SuccessAsOK(result);
        }
    }
}
