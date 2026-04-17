using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SertifikaParmakIziFeatures.GetSertifikaParmakIzi
{
    public class GetSertifikaParmakIziQueryHandler : IRequestHandler<GetSertifikaParmakIziQuery, ServiceResult>
    {
        private readonly ISertifikaParmakIziRepository _sertifikaParmakIziRepository;

        public GetSertifikaParmakIziQueryHandler(ISertifikaParmakIziRepository sertifikaParmakIziRepository)
        {
            _sertifikaParmakIziRepository = sertifikaParmakIziRepository;
        }

        public async Task<ServiceResult> Handle(GetSertifikaParmakIziQuery request, CancellationToken cancellationToken)
        {
            var entity = await _sertifikaParmakIziRepository.GetByIdAsync(request.Id);
            if (entity == null)
                return ServiceResult.ErrorAsNotFound();
            return ServiceResult.SuccessAsOK(entity);
        }
    }
}
