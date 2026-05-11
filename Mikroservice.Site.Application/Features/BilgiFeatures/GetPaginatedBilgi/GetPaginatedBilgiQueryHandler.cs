using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.BilgiDtos;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.GetPaginatedBilgi
{
    public class GetPaginatedBilgiQueryHandler(
        IBilgiRepository bilgiRepository,
        IRedisCacheService redis,
        ILogger<GetPaginatedBilgiQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetPaginatedBilgiQuery, ServiceResult<PaginatedResult<BilgiDto>>>
    {
        public async Task<ServiceResult<PaginatedResult<BilgiDto>>> Handle(
            GetPaginatedBilgiQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey =
                $"bilgi:list:" +
                $"{request.SiteId}:{request.DilId}:" +
                $"p:{request.Page}:" +
                $"ps:{request.PageSize}:" +
                $"s:{request.Search}:" +
                $"ob:{request.OrderBy}:" +
                $"od:{request.OrderDir}";

            var cachedResult =
                await redis.GetAsync<PaginatedResult<BilgiDto>>(cacheKey, cancellationToken);

            if (cachedResult is not null)
            {
                logger.LogInformation("Bilgi listesi cache'den getirildi");
                return ServiceResult<PaginatedResult<BilgiDto>>.SuccessAsOK(cachedResult);
            }

            IQueryable<Domain.Entities.Bilgi> query = bilgiRepository
                .GetAll()
                .Where(x => x.SiteId == request.SiteId && x.DilId == request.DilId && !x.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();
                query = query.Where(x =>
                    x.Baslik.ToLower().Contains(search) ||
                    x.KisaAciklama.ToLower().Contains(search) ||
                    x.IcerikMetni.ToLower().Contains(search));
            }

            query = (request.OrderBy?.ToLower(), request.OrderDir?.ToLower()) switch
            {
                ("baslik", "asc") => query.OrderBy(x => x.Baslik),
                ("baslik", "desc") => query.OrderByDescending(x => x.Baslik),
                ("yayimtarihi", "asc") => query.OrderBy(x => x.YayimTarihi),
                ("yayimtarihi", "desc") => query.OrderByDescending(x => x.YayimTarihi),
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
                .ProjectTo<BilgiDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<BilgiDto>
            {
                Data = data,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            await redis.SetAsync(cacheKey, result, TimeSpan.FromMinutes(5), cancellationToken);

            logger.LogInformation(
                "Bilgi listesi DB'den getirildi. TotalCount: {TotalCount}, Page: {Page}",
                totalCount, request.Page);

            return ServiceResult<PaginatedResult<BilgiDto>>.SuccessAsOK(result);
        }
    }
}