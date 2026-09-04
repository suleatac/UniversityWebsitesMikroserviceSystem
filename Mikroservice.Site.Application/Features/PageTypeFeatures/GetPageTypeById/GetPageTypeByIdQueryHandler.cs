using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.PageTypeDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypeById
{
    public class GetPageTypeByIdQueryHandler(IPageTypeRepository repository)
        : IRequestHandler<GetPageTypeByIdQuery, ServiceResult<PageTypeDto>>
    {
        public async Task<ServiceResult<PageTypeDto>> Handle(GetPageTypeByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync(request.Id);
            if (entity is null)
                return ServiceResult<PageTypeDto>.Error("PageType bulunamadı", HttpStatusCode.NotFound);

            return ServiceResult<PageTypeDto>.SuccessAsOK(new PageTypeDto
            {
                Id = entity.Id,
                PageTypeKind = entity.PageTypeKind,
                DilId = entity.DilId,
                Name = entity.Name,
                Slug = entity.Slug,
                TemplateId = entity.TemplateId,
                ViewName = entity.ViewName,
                IsHomePage = entity.IsHomePage
            });
        }
    }
}