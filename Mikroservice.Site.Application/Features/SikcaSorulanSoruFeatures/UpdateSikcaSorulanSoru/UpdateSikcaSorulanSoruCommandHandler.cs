using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.UpdateSikcaSorulanSoru
{
    public class UpdateSikcaSorulanSoruCommandHandler : IRequestHandler<UpdateSikcaSorulanSoruCommand, ServiceResult>
    {
        private readonly ISikcaSorulanSoruRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSikcaSorulanSoruCommandHandler(ISikcaSorulanSoruRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Handle(UpdateSikcaSorulanSoruCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                return ServiceResult.ErrorAsNotFound();
            entity.SiteId = request.SiteId;
            entity.DilId = request.DilId;
            entity.KategoriId = request.KategoriId;
            entity.Soru = request.Soru;
            entity.Cevap = request.Cevap;
            entity.Sira = request.Sira;
            entity.IsDeleted = request.IsDeleted;
            entity.SeoUrl = request.SeoUrl;
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.SuccessAsNoContent();
        }
    }
}
