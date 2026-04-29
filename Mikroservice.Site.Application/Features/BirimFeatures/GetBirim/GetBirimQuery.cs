using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.BirimDtos;

namespace Mikroservice.Site.Application.Features.BirimFeatures.GetBirim
{
    public record GetBirimQuery : IRequestByServiceResult<List<BirimDto>>;
}
