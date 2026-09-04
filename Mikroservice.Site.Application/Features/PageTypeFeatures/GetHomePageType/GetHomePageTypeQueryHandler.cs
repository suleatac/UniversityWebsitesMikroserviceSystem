using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.PageTypeDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.GetHomePageType
{
    public class GetHomePageTypeQueryHandler(IPageTypeRepository repository)
        : IRequestHandler<GetHomePageTypeQuery, ServiceResult<PageTypeDto>>
    {
        public Task<ServiceResult<PageTypeDto>> Handle(GetHomePageTypeQuery request, CancellationToken cancellationToken)
        {
            var pageType = repository.GetAll()
                .FirstOrDefault(x => x.TemplateId == request.SiteTemplateId && x.DilId == request.DilId && x.IsHomePage);

            if (pageType is null)
                return Task.FromResult(ServiceResult<PageTypeDto>.Error("Ana PageType bulunamadı", HttpStatusCode.NotFound));

            return Task.FromResult(ServiceResult<PageTypeDto>.SuccessAsOK(new PageTypeDto
            {
                Id = pageType.Id,
                PageTypeKind = pageType.PageTypeKind,
                TemplateId = pageType.TemplateId,
                DilId = pageType.DilId,
                Name = pageType.Name,
                Slug = pageType.Slug,
                ViewName = pageType.ViewName,
                IsHomePage = pageType.IsHomePage
            }));
        }
    }
}