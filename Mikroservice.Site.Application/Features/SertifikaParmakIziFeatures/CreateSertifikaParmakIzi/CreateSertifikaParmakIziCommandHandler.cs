using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.CreateSertifikaParmakIzi
{
    public class CreateSertifikaParmakIziCommandHandler : IRequestHandler<CreateSertifikaParmakIziCommand, ServiceResult>
    {
        private readonly ISertifikaParmakIziRepository _sertifikaParmakIziRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSertifikaParmakIziCommandHandler(ISertifikaParmakIziRepository sertifikaParmakIziRepository, IUnitOfWork unitOfWork)
        {
            _sertifikaParmakIziRepository = sertifikaParmakIziRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Handle(CreateSertifikaParmakIziCommand request, CancellationToken cancellationToken)
        {
            var entity = new SertifikaParmakIzi
            {
                SertifikaParmakIziNumarasi = request.SertifikaParmakIziNumarasi,
                SertifikaYili = request.SertifikaYili,
                Aktif = request.Aktif
            };
            await _sertifikaParmakIziRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.SuccessAsNoContent();
        }
    }
}
