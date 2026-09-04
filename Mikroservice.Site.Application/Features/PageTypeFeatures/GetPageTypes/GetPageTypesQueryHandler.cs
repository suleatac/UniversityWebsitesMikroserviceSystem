using MediatR;
using Microsoft.EntityFrameworkCore;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.PageTypeDtos;

namespace Mikroservice.Site.Application.Features.PageTypeFeatures.GetPageTypes
{
    public class GetPageTypesQueryHandler(IPageTypeRepository repository)
        : IRequestHandler<GetPageTypesQuery, ServiceResult<List<PageTypeDto>>>
    {
        public async Task<ServiceResult<List<PageTypeDto>>> Handle(GetPageTypesQuery request, CancellationToken cancellationToken)
        {
            var pageTypes = await repository.GetAll()
                .OrderBy(x => x.Id)
                .ThenBy(x => x.Name)
                .Select(x => new PageTypeDto
                {
                    Id = x.Id,
                    PageTypeKind = x.PageTypeKind,
                    DilId = x.DilId,
                    Name = x.Name,
                    Slug = x.Slug,
                    TemplateId = x.TemplateId,
                    ViewName = x.ViewName,
                    IsHomePage = x.IsHomePage
                })
                .ToListAsync(cancellationToken);

            return ServiceResult<List<PageTypeDto>>.SuccessAsOK(pageTypes);
        }
    }
}