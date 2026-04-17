using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.DeleteSikcaSorulanSoru
{
    public class DeleteSikcaSorulanSoruCommandHandler : IRequestHandler<DeleteSikcaSorulanSoruCommand, ServiceResult>
    {
        private readonly ISikcaSorulanSoruRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSikcaSorulanSoruCommandHandler(ISikcaSorulanSoruRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Handle(DeleteSikcaSorulanSoruCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                return ServiceResult.ErrorAsNotFound();
            _repository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.SuccessAsNoContent();
        }
    }
}
