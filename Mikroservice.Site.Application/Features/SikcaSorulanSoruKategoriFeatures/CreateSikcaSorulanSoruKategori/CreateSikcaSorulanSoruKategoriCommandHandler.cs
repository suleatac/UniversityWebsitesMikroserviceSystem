using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.CreateSikcaSorulanSoruKategori
{
    public class CreateSikcaSorulanSoruKategoriCommandHandler : IRequestHandler<CreateSikcaSorulanSoruKategoriCommand, ServiceResult>
    {
        private readonly ISikcaSorulanSoruKategoriRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSikcaSorulanSoruKategoriCommandHandler(ISikcaSorulanSoruKategoriRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Handle(CreateSikcaSorulanSoruKategoriCommand request, CancellationToken cancellationToken)
        {
            var entity = new SikcaSorulanSoruKategori
            {
                Ad = request.Ad,
                Sira = request.Sira,
                IsDeleted = request.IsDeleted
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.SuccessAsNoContent();
        }
    }
}
