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
                .FirstOrDefault(x => x.SiteId == request.SiteId && x.DilId == request.DilId && x.IsActive && x.IsHomePage);

            if (pageType is null)
                return Task.FromResult(ServiceResult<PageTypeDto>.Error("Ana PageType bulunamadı", HttpStatusCode.NotFound));

            return Task.FromResult(ServiceResult<PageTypeDto>.SuccessAsOK(new PageTypeDto
            {
                Id = pageType.Id,
                PageTypeId = pageType.PageTypeId,
                SiteId = pageType.SiteId,
                DilId = pageType.DilId,
                Name = pageType.Name,
                Slug = pageType.Slug,
                TemplateId = pageType.TemplateId,
                ViewName = pageType.ViewName,
                IsHomePage = pageType.IsHomePage,
                IsActive = pageType.IsActive
            }));
        }
    }
}