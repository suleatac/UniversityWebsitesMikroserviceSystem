using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microservice.Site.Application.Contracts.IRepositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.PopupDtos;

namespace Mikroservice.Site.Application.Features.PopupFeatures.GetPaginatedPopup
{
    public class GetPaginatedPopupQueryHandler(
        IPopupRepository popupRepository,
        IRedisCacheService redis,
        ILogger<GetPaginatedPopupQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetPaginatedPopupQuery, ServiceResult<PaginatedResult<PopupDto>>>
    {
        public async Task<ServiceResult<PaginatedResult<PopupDto>>> Handle(
            GetPaginatedPopupQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey =
                $"popup:list:" +
                $"{request.SiteId}:{request.DilId}:" +
                $"p:{request.Page}:" +
                $"ps:{request.PageSize}:" +
                $"s:{request.Search}:" +
                $"ob:{request.OrderBy}:" +
                $"od:{request.OrderDir}";

            var cachedResult =
                await redis.GetAsync<PaginatedResult<PopupDto>>(cacheKey, cancellationToken);

            if (cachedResult is not null)
            {
                logger.LogInformation("Popup listesi cache'den getirildi");
                return ServiceResult<PaginatedResult<PopupDto>>.SuccessAsOK(cachedResult);
            }

            IQueryable<Domain.Entities.Popup> query = popupRepository
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
                ("kisaaciklama", "asc") => query.OrderBy(x => x.KisaAciklama),
                ("kisaaciklama", "desc") => query.OrderByDescending(x => x.KisaAciklama),
                _ => request.OrderDir?.ToLower() == "asc"
                    ? query.OrderBy(x => x.Id)
                    : query.OrderByDescending(x => x.Id)
            };

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ProjectTo<PopupDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<PopupDto>
            {
                Data = items,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            await redis.SetAsync(cacheKey, result, TimeSpan.FromHours(1), cancellationToken);

            logger.LogInformation("Popup listesi DB'den getirildi. Count: {Count}", totalCount);

            return ServiceResult<PaginatedResult<PopupDto>>.SuccessAsOK(result);
        }
    }
}