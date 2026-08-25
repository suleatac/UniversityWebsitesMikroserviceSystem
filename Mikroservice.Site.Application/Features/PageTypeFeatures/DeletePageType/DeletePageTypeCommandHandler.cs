using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Microservice.Site.Application.Features.PageTypeFeatures.DeletePageType;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.DeletePageType
{
    public class DeletePageTypeCommandHandler(IPageTypeRepository pageTypeRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<DeletePageTypeCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(DeletePageTypeCommand request, CancellationToken cancellationToken)
        {
            var pageType = await pageTypeRepository.GetByIdAsync(request.Id);
            if (pageType is null)
                return ServiceResult.ErrorAsNotFound();

            pageType.IsDeleted = true;
            pageType.IsActive = false;
            pageTypeRepository.Update(pageType);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ServiceResult.Success();
        }
    }
}