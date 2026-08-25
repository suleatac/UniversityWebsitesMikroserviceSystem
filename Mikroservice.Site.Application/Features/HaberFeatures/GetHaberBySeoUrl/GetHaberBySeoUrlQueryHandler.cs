using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.HaberDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.HaberFeatures.GetHaberBySeoUrl
{
    public class GetHaberBySeoUrlQueryHandler(IHaberRepository repository, IMapper mapper)
        : IRequestHandler<GetHaberBySeoUrlQuery, ServiceResult<HaberDetailDto>>
    {
        public async Task<ServiceResult<HaberDetailDto>> Handle(GetHaberBySeoUrlQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetBySeoUrlAsync(request.SiteId, request.DilId, request.SeoUrl, cancellationToken);
            return entity is null
                ? ServiceResult<HaberDetailDto>.Error("Haber bulunamadı", HttpStatusCode.NotFound)
                : ServiceResult<HaberDetailDto>.SuccessAsOK(mapper.Map<HaberDetailDto>(entity));
        }
    }
}