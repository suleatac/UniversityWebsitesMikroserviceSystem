using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.DeleteSertifikaParmakIzi
{
    public class DeleteSertifikaParmakIziCommandHandler : IRequestHandler<DeleteSertifikaParmakIziCommand, ServiceResult>
    {
        private readonly ISertifikaParmakIziRepository _sertifikaParmakIziRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSertifikaParmakIziCommandHandler(ISertifikaParmakIziRepository sertifikaParmakIziRepository, IUnitOfWork unitOfWork)
        {
            _sertifikaParmakIziRepository = sertifikaParmakIziRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Handle(DeleteSertifikaParmakIziCommand request, CancellationToken cancellationToken)
        {
            var entity = await _sertifikaParmakIziRepository.GetByIdAsync(request.Id);
            if (entity == null)
                return ServiceResult.ErrorAsNotFound();
            _sertifikaParmakIziRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.SuccessAsNoContent();
        }
    }
}
