using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microservice.Shared;
using Microservice.Shared.Services.RedisServiceItems;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs;
using Mikroservice.Site.Application.DTOs.AuditLogDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.AuditLogFeatures.GetPaginatedAuditLog
{
    public class GetPaginatedAuditLogQueryHandler(
        IAuditLogRepository auditLogRepository,
        ILogger<GetPaginatedAuditLogQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetPaginatedAuditLogQuery, ServiceResult<PaginatedResult<AuditLogDto>>>
    {
        public async Task<ServiceResult<PaginatedResult<AuditLogDto>>> Handle(
            GetPaginatedAuditLogQuery request,
            CancellationToken cancellationToken)
        {


            IQueryable<AuditLog> query = auditLogRepository.GetAll();

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search;
                query = query.Where(x =>
                    x.UserId != null && x.UserId.Contains(search) ||
                    x.Username.Contains(search) ||
                    x.Action.Contains(search) ||
                    x.IpAddress.Contains(search) ||
                    x.EntityName != null && x.EntityName.Contains(search) ||
                    x.Description != null && x.Description.Contains(search));
            }

            // Date range filtering
            if (request.StartDate.HasValue)
            {
                var start = request.StartDate.Value.Date;
                query = query.Where(x => x.CreatedAt >= start);
            }

            if (request.EndDate.HasValue)
            {
                var end = request.EndDate.Value.Date.AddDays(1);
                query = query.Where(x => x.CreatedAt < end);
            }

            query = (request.OrderBy?.ToLower(), request.OrderDir?.ToLower()) switch {
                ("userid", "asc") => query.OrderBy(x => x.UserId),
                ("userid", "desc") => query.OrderByDescending(x => x.UserId),
                ("username", "asc") => query.OrderBy(x => x.Username),
                ("username", "desc") => query.OrderByDescending(x => x.Username),
                ("action", "asc") => query.OrderBy(x => x.Action),
                ("action", "desc") => query.OrderByDescending(x => x.Action),
                ("ipaddress", "asc") => query.OrderBy(x => x.IpAddress),
                ("ipaddress", "desc") => query.OrderByDescending(x => x.IpAddress),
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
                .ProjectTo<AuditLogDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

            var result = new PaginatedResult<AuditLogDto> {
                Data = data,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

          

            logger.LogInformation(
                "AuditLog listesi DB'den getirildi. TotalCount: {TotalCount}, Page: {Page}",
                totalCount, request.Page);

            return ServiceResult<PaginatedResult<AuditLogDto>>.SuccessAsOK(result);
        }
    }
}
