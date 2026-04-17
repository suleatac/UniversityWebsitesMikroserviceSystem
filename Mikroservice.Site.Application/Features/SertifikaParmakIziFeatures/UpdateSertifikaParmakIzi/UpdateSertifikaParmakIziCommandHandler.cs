using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.UpdateSertifikaParmakIzi
{
    public class UpdateSertifikaParmakIziCommandHandler : IRequestHandler<UpdateSertifikaParmakIziCommand, ServiceResult>
    {
        private readonly ISertifikaParmakIziRepository _sertifikaParmakIziRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSertifikaParmakIziCommandHandler(ISertifikaParmakIziRepository sertifikaParmakIziRepository, IUnitOfWork unitOfWork)
        {
            _sertifikaParmakIziRepository = sertifikaParmakIziRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Handle(UpdateSertifikaParmakIziCommand request, CancellationToken cancellationToken)
        {
            var entity = await _sertifikaParmakIziRepository.GetByIdAsync(request.Id);
            if (entity == null)
                return ServiceResult.ErrorAsNotFound();
            entity.SertifikaParmakIziNumarasi = request.SertifikaParmakIziNumarasi;
            entity.SertifikaYili = request.SertifikaYili;
            entity.Aktif = request.Aktif;
            _sertifikaParmakIziRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.SuccessAsNoContent();
        }
    }
}
