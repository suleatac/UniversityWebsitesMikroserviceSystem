using AutoMapper;
using MediatR;
using Microservice.Shared;
using Microservice.Site.Application.Contracts.IRepositories;
using Mikroservice.Site.Application.DTOs.YonetimDuyuru;

namespace Mikroservice.Site.Application.Features.YonetimDuyuruFeatures.GetYonetimDuyuruDetail
{
    public class GetYonetimDuyuruDetailQueryHandler(
        IYonetimDuyuruRepository yonetimDuyuruRepository,
        IYonetimDuyuruOkunduRepository yonetimDuyuruOkunduRepository,
        IMapper mapper
        ) : IRequestHandler<GetYonetimDuyuruDetailQuery, ServiceResult<YonetimDuyuruDetailDto>>
    {
        public async Task<ServiceResult<YonetimDuyuruDetailDto>> Handle(GetYonetimDuyuruDetailQuery request, CancellationToken cancellationToken)
        {
            var duyuru = await yonetimDuyuruRepository.GetByIdAsync(request.Id);
            if (duyuru is null)
                return ServiceResult<YonetimDuyuruDetailDto>.Error("Duyuru bulunamadı", System.Net.HttpStatusCode.NotFound);

            var dto = mapper.Map<YonetimDuyuruDetailDto>(duyuru);

            // Kullanıcının bu duyuruyu okuyup okumadığını kontrol et
            dto.OkunduMu = await yonetimDuyuruOkunduRepository.HasUserReadAsync(request.Id, request.KeycloakUserId, cancellationToken);

            return ServiceResult<YonetimDuyuruDetailDto>.SuccessAsOK(dto);
        }
    }
}
