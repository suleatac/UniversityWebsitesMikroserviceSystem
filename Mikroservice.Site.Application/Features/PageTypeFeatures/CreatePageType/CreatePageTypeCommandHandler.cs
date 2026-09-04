using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.CreatePageType
{
    public class CreatePageTypeCommandHandler(IPageTypeRepository pageTypeRepository, IUnitOfWork unitOfWork)
        : IRequestHandler<CreatePageTypeCommand, ServiceResult<CreatePageTypeResponse>>
    {
        public async Task<ServiceResult<CreatePageTypeResponse>> Handle(CreatePageTypeCommand request, CancellationToken cancellationToken)
        {
            var pageType = new PageType
            {
                PageTypeKind = request.PageTypeKind,
                Name = request.Name,
                Slug = request.Slug,
                TemplateId = request.TemplateId,
                DilId = request.DilId,
                ViewName = request.ViewName,
                IsHomePage = request.IsHomePage
            };

            await pageTypeRepository.AddAsync(pageType);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return ServiceResult<CreatePageTypeResponse>.SuccessAsCreated(
                new CreatePageTypeResponse(pageType.Id), $"/api/v1/page-types/{pageType.Id}");
        }
    }
}