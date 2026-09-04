using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.PageTypeDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypeBySlug
{
    public class GetPageTypeBySlugQueryHandler(IPageTypeRepository repository)
        : IRequestHandler<GetPageTypeBySlugQuery, ServiceResult<PageTypeDto>>
    {
        public Task<ServiceResult<PageTypeDto>> Handle(GetPageTypeBySlugQuery request, CancellationToken cancellationToken)
        {
            var pageType = repository.GetAll()
                .FirstOrDefault(x => x.TemplateId == request.TemplateId && x.DilId == request.DilId &&
                    x.Slug.ToLower() == request.Slug.ToLower());

            if (pageType is null)
                return Task.FromResult(ServiceResult<PageTypeDto>.Error("PageType bulunamadı", HttpStatusCode.NotFound));

            return Task.FromResult(ServiceResult<PageTypeDto>.SuccessAsOK(new PageTypeDto
            {
                Id = pageType.Id,
                PageTypeKind = pageType.PageTypeKind,
                DilId = pageType.DilId,
                Name = pageType.Name,
                Slug = pageType.Slug,
                TemplateId = pageType.TemplateId,
                ViewName = pageType.ViewName,
                IsHomePage = pageType.IsHomePage,
            }));
        }
    }
}