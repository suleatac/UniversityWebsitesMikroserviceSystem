using MediatR;
using Microservice.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.AuditLogDtos;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.AuditLogFeatures.GetAuditLogDailyStats
{
    public class GetAuditLogDailyStatsQueryHandler(
        IAuditLogRepository auditLogRepository,
        ILogger<GetAuditLogDailyStatsQueryHandler> logger
    ) : IRequestHandler<GetAuditLogDailyStatsQuery, ServiceResult<List<AuditLogDailyStatDto>>>
    {
        public async Task<ServiceResult<List<AuditLogDailyStatDto>>> Handle(
            GetAuditLogDailyStatsQuery request,
            CancellationToken cancellationToken)
        {
            IQueryable<AuditLog> query = auditLogRepository.GetAll();

            // Filter by date range
            var start = request.StartDate.Date;
            var end = request.EndDate.Date.AddDays(1);
            query = query.Where(x => x.CreatedAt >= start && x.CreatedAt < end);

            // Group by date and count
            var dailyStats = await query
                .GroupBy(x => new { x.CreatedAt.Year, x.CreatedAt.Month, x.CreatedAt.Day })
                .Select(g => new AuditLogDailyStatDto
                {
                    Date = new DateTime(g.Key.Year, g.Key.Month, g.Key.Day),
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync(cancellationToken);

            logger.LogInformation(
                "Günlük audit log istatistikleri getirildi. {StartDate} - {EndDate}, {DayCount} gün",
                request.StartDate.ToString("yyyy-MM-dd"),
                request.EndDate.ToString("yyyy-MM-dd"),
                dailyStats.Count);

            return ServiceResult<List<AuditLogDailyStatDto>>.SuccessAsOK(dailyStats);
        }
    }
}
