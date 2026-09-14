using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.BilgiDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.GetBilgiBySeoUrl
{
    public class GetBilgiBySeoUrlQueryHandler(IBilgiRepository repository, IMapper mapper)
        : IRequestHandler<GetBilgiBySeoUrlQuery, ServiceResult<BilgiDetailDto>>
    {
        public async Task<ServiceResult<BilgiDetailDto>> Handle(GetBilgiBySeoUrlQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetBySeoUrlAsync(request.SiteId, request.DilId, request.SeoUrl, cancellationToken);
            return entity is null
                ? ServiceResult<BilgiDetailDto>.Error("Bilgi bulunamadı", HttpStatusCode.NotFound)
                : ServiceResult<BilgiDetailDto>.SuccessAsOK(mapper.Map<BilgiDetailDto>(entity));
        }
    }
}
