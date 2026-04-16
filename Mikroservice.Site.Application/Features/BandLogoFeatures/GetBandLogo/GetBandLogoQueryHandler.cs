using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.GetBandLogo
{
    public class GetBandLogoQueryHandler : IRequestHandler<GetBandLogoQuery, ServiceResult>
    {
        private readonly IBandLogoRepository _bandLogoRepository;

        public GetBandLogoQueryHandler(IBandLogoRepository bandLogoRepository)
        {
            _bandLogoRepository = bandLogoRepository;
        }

        public async Task<ServiceResult> Handle(GetBandLogoQuery request, CancellationToken cancellationToken)
        {
            var entity = await _bandLogoRepository.GetByIdAsync(request.Id);
            if (entity == null)
                return ServiceResult.ErrorAsNotFound();
            return ServiceResult.SuccessAsOK(entity);
        }
    }
}
