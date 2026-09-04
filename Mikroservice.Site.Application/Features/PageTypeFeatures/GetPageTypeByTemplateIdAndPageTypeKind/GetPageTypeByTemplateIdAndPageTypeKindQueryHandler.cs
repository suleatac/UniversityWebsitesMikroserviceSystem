using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.PageTypeDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypeByTemplateIdAndPageTypeKind
{
    public class GetPageTypeByTemplateIdAndPageTypeKindQueryHandler(IPageTypeRepository repository)
        : IRequestHandler<GetPageTypeByTemplateIdAndPageTypeKindQuery, ServiceResult<PageTypeDto>>
    {
        public async Task<ServiceResult<PageTypeDto>> Handle(
            GetPageTypeByTemplateIdAndPageTypeKindQuery request,
            CancellationToken cancellationToken)
        {
            var pageType = await repository.GetPageTypeByTemplateIdAndPageTypeKind(request.TemplateId, request.PageTypeKind, request.DilId, cancellationToken);

            if (pageType is null)
                return ServiceResult<PageTypeDto>.Error("PageType bulunamadı", HttpStatusCode.NotFound);

            return ServiceResult<PageTypeDto>.SuccessAsOK(new PageTypeDto
            {
                Id = pageType.Id,
                PageTypeKind = pageType.PageTypeKind,
                DilId = pageType.DilId,
                Name = pageType.Name,
                Slug = pageType.Slug,
                TemplateId = pageType.TemplateId,
                ViewName = pageType.ViewName,
                IsHomePage = pageType.IsHomePage,
            });
        }
    }
}
