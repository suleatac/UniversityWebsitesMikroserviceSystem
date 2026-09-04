using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;

namespace Microservice.Site.Application.Features.PageTypeFeatures.UpdatePageType
{
    public class UpdatePageTypeCommandHandler(IPageTypeRepository pageTypeRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<UpdatePageTypeCommand, ServiceResult>
    {
        public async Task<ServiceResult> Handle(UpdatePageTypeCommand request, CancellationToken cancellationToken)
        {
            var pageType = await pageTypeRepository.GetByIdAsync(request.Id);
            if (pageType is null)
                return ServiceResult.ErrorAsNotFound();
            pageType.PageTypeKind = request.PageTypeKind;
            pageType.Name = request.Name;
            pageType.Slug = request.Slug;
            pageType.TemplateId = request.TemplateId;
            pageType.DilId = request.DilId;
            pageType.ViewName = request.ViewName;
            pageType.IsHomePage = request.IsHomePage;

            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ServiceResult.Success();
        }
    }
}