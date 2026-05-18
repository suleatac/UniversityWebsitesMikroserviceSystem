using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.AuditLogFeatures.DeleteAuditLog
{
    public class DeleteAuditLogCommandHandler(
          IAuditLogRepository auditLogRepository,
          IUnitOfWork unitOfWork
        )
        : IRequestHandler<DeleteAuditLogCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeleteAuditLogCommand request, CancellationToken cancellationToken)
        {
            var auditLog = await auditLogRepository.GetByIdAsync(request.Id);
            if (auditLog == null)
            {
                return ServiceResult.ErrorAsNotFound();
            }

           
            auditLogRepository.Delete(auditLog);
            await unitOfWork.SaveChangesAsync(cancellationToken);


            return ServiceResult.Success();
        }
    }
}
