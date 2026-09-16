using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.VideoDtos;
using System.Net;

namespace Mikroservice.Site.Application.Features.VideoFeatures.GetVideoBySeoUrl
{
    public class GetVideoBySeoUrlQueryHandler(IVideoRepository repository, IMapper mapper)
        : IRequestHandler<GetVideoBySeoUrlQuery, ServiceResult<VideoDetailDto>>
    {
        public async Task<ServiceResult<VideoDetailDto>> Handle(GetVideoBySeoUrlQuery request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetBySeoUrlAsync(request.SiteId, request.DilId, request.SeoUrl, cancellationToken);
            return entity is null
                ? ServiceResult<VideoDetailDto>.Error("Video bulunamadı", HttpStatusCode.NotFound)
                : ServiceResult<VideoDetailDto>.SuccessAsOK(mapper.Map<VideoDetailDto>(entity));
        }
    }
}
