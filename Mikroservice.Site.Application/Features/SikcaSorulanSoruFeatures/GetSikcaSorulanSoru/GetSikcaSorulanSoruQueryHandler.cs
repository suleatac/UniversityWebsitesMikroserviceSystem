using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.GetSikcaSorulanSoru
{
    public class GetSikcaSorulanSoruQueryHandler : IRequestHandler<GetSikcaSorulanSoruQuery, ServiceResult>
    {
        private readonly ISikcaSorulanSoruRepository _repository;

        public GetSikcaSorulanSoruQueryHandler(ISikcaSorulanSoruRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetSikcaSorulanSoruQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                return ServiceResult.ErrorAsNotFound();
            return ServiceResult.SuccessAsOK(entity);
        }
    }
}
