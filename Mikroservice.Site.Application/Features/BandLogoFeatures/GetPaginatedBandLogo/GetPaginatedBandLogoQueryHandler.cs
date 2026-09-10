using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.BandLogoDtos;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.GetPaginatedBandLogo
{
    public class GetPaginatedBandLogoQueryHandler(
        IBandLogoRepository bandLogoRepository,
        IRedisCacheService redis,
        ILogger<GetPaginatedBandLogoQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetPaginatedBandLogoQuery, ServiceResult<PaginatedResult<BandLogoDto>>>
    {
        public async Task<ServiceResult<PaginatedResult<BandLogoDto>>> Handle(
            GetPaginatedBandLogoQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey =
                $"bandlogo:list:" +
                $"{request.SiteId}:{request.DilId}:" +
                $"p:{request.Page}:" +
                $"ps:{request.PageSize}:" +
                $"s:{request.Search}:" +
                $"ob:{request.OrderBy}:" +
                $"od:{request.OrderDir}";

            var cachedResult =
                await redis.GetAsync<PaginatedResult<BandLogoDto>>(cacheKey, cancellationToken);

            if (cachedResult is not null)
            {
                logger.LogInformation("BandLogo listesi cache'den getirildi");
                return ServiceResult<PaginatedResult<BandLogoDto>>.SuccessAsOK(cachedResult);
            }

            IQueryable<Domain.Entities.BandLogo> query = bandLogoRepository
                .GetAll()
                .Where(x => x.SiteId == request.SiteId && x.DilId == request.DilId);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();
                query = query.Where(x =>
                    x.Ad.ToLower().Contains(search) ||
                    (x.Link != null && x.Link.ToLower().Contains(search)));
            }

            query = (request.OrderBy?.ToLower(), request.OrderDir?.ToLower()) switch
            {
                ("ad", "asc") => query.OrderBy(x => x.Ad),
                ("ad", "desc") => query.OrderByDescending(x => x.Ad),
                ("eklenmetarihi", "asc") => query.OrderBy(x => x.EklenmeTarihi),
                ("eklenmetarihi", "desc") => query.OrderByDescending(x => x.EklenmeTarihi),
                _ => query.OrderByDescending(x => x.Id)
            };

            var totalCount = await query.CountAsync(cancellationToken);

            if (request.PageSize != -1)
            {
                query = query
                    .Skip((request.Page - 1) * request.PageSize)
                    .Take(request.PageSize);
            }

            var data = await query
                .ProjectTo<BandLogoDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<BandLogoDto>
            {
                Data = data,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            await redis.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5), cancellationToken);

            logger.LogInformation(
                "BandLogo listesi DB'den getirildi. TotalCount: {TotalCount}, Page: {Page}",
                totalCount, request.Page);

            return ServiceResult<PaginatedResult<BandLogoDto>>.SuccessAsOK(result);
        }
    }
}
