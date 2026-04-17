using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.UpdateSikcaSorulanSoruKategori
{
    public class UpdateSikcaSorulanSoruKategoriCommandHandler : IRequestHandler<UpdateSikcaSorulanSoruKategoriCommand, ServiceResult>
    {
        private readonly ISikcaSorulanSoruKategoriRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSikcaSorulanSoruKategoriCommandHandler(ISikcaSorulanSoruKategoriRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Handle(UpdateSikcaSorulanSoruKategoriCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                return ServiceResult.ErrorAsNotFound();
            entity.Ad = request.Ad;
            entity.Sira = request.Sira;
            entity.IsDeleted = request.IsDeleted;
            _repository.Update(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.SuccessAsNoContent();
        }
    }
}
