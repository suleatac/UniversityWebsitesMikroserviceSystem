using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.GetSikcaSorulanSoruKategori
{
    public class GetSikcaSorulanSoruKategoriQueryHandler : IRequestHandler<GetSikcaSorulanSoruKategoriQuery, ServiceResult>
    {
        private readonly ISikcaSorulanSoruKategoriRepository _repository;

        public GetSikcaSorulanSoruKategoriQueryHandler(ISikcaSorulanSoruKategoriRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(GetSikcaSorulanSoruKategoriQuery request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                return ServiceResult.ErrorAsNotFound();
            return ServiceResult.SuccessAsOK(entity);
        }
    }
}
