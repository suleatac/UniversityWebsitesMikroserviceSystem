using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.DuyuruDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.GetDuyuruBySeoUrl
{
    public class GetDuyuruBySeoUrlQueryHandler(IDuyuruRepository repository, IMapper mapper)
        : IRequestHandler<GetDuyuruBySeoUrlQuery, ServiceResult<DuyuruDetailDto>>
    {
        public async Task<ServiceResult<DuyuruDetailDto>> Handle(GetDuyuruBySeoUrlQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetBySeoUrlAsync(request.SiteId, request.DilId, request.SeoUrl, cancellationToken);
            return entity is null
                ? ServiceResult<DuyuruDetailDto>.Error("Duyuru bulunamadı", HttpStatusCode.NotFound)
                : ServiceResult<DuyuruDetailDto>.SuccessAsOK(mapper.Map<DuyuruDetailDto>(entity));
        }
    }
}