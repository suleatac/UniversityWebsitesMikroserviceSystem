using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microsoft.Extensions.Logging;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.AuditLogDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.AuditLogFeatures.GetAuditLogById
{
    public class GetAuditLogByIdQueryHandler(
        IAuditLogRepository auditLogRepository,
        ILogger<GetAuditLogByIdQueryHandler> logger,
        IMapper mapper
    ) : IRequestHandler<GetAuditLogByIdQuery, ServiceResult<AuditLogDetailDto>>
    {
        public async Task<ServiceResult<AuditLogDetailDto>> Handle(GetAuditLogByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await auditLogRepository.GetByIdAsync(request.Id);

            if (entity is null)
            {
                logger.LogWarning("AuditLog bulunamadı. Id: {Id}", request.Id);
                return ServiceResult<AuditLogDetailDto>.Error("AuditLog bulunamadı", HttpStatusCode.NotFound);
            }

            var dto = mapper.Map<AuditLogDetailDto>(entity);

            logger.LogInformation("AuditLog DB'den alındı. Id: {Id}", request.Id);

            return ServiceResult<AuditLogDetailDto>.SuccessAsOK(dto);
        }
    }
}
