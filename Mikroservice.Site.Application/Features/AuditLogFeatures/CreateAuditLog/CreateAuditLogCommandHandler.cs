using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.AuditLogFeatures.CreateAuditLog
{
    public class CreateAuditLogCommandHandler(
          IAuditLogRepository auditLogRepository,
          IUnitOfWork unitOfWork
        )
        : IRequestHandler<CreateAuditLogCommand, ServiceResult<CreateAuditLogResponse>>
    {
        public async Task<ServiceResult<CreateAuditLogResponse>> Handle(CreateAuditLogCommand request, CancellationToken cancellationToken)
        {
            var newAuditLog = new AuditLog {
                Description = request.Description,
                IpAddress = request.IpAddress,
                UserId = request.UserId,
                Username = request.Username,
                Action = request.Action,
                EntityName = request.EntityName,
                EntityId = request.EntityId,
                SiteId = request.SiteId,
                CreatedAt = DateTime.Now
            };
            await auditLogRepository.AddAsync(newAuditLog);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            var response = new CreateAuditLogResponse(newAuditLog.Id);
            return ServiceResult<CreateAuditLogResponse>
            .SuccessAsCreated(response, $"/api/v1/audit-logs/{newAuditLog.Id}");
        }
    }
}

