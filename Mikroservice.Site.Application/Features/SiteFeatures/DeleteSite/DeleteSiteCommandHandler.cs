using MediatR;
using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Mikroservice.Site.Application.Features.SiteFeatures.DeleteSite
{
    public class DeleteSiteCommandHandler : IRequestHandler<DeleteSiteCommand, ServiceResult>
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteSiteCommandHandler(ISiteRepository siteRepository, IUnitOfWork unitOfWork)
        {
            _siteRepository = siteRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult> Handle(DeleteSiteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _siteRepository.GetByIdAsync(request.Id);
            if (entity == null)
                return ServiceResult.ErrorAsNotFound();
            _siteRepository.Delete(entity);
            await _unitOfWork.SaveChangesAsync();
            return ServiceResult.SuccessAsNoContent();
        }
    }
}
