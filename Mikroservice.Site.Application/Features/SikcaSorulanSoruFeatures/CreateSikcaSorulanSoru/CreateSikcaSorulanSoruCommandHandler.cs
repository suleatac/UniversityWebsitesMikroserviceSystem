using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.CreateSikcaSorulanSoru
{
    public class CreateSikcaSorulanSoruCommandHandler : IRequestHandler<CreateSikcaSorulanSoruCommand, ServiceResult>
    {
        private readonly ISikcaSorulanSoruRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSikcaSorulanSoruCommandHandler(ISikcaSorulanSoruRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Handle(CreateSikcaSorulanSoruCommand request, CancellationToken cancellationToken)
        {
            var entity = new SikcaSorulanSoru
            {
                SiteId = request.SiteId,
                DilId = request.DilId,
                KategoriId = request.KategoriId,
                Soru = request.Soru,
                Cevap = request.Cevap,
                Sira = request.Sira,
                IsDeleted = request.IsDeleted,
                SeoUrl = request.SeoUrl
            };
            await _repository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.SuccessAsNoContent();
        }
    }
}
