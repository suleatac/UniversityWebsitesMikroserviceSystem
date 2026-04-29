using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.BirimDtos;

namespace Mikroservice.Site.Application.Features.BirimFeatures.GetBirimById
{
    public record GetBirimByIdQuery(int Id) : IRequestByServiceResult<BirimDetailDto>;
}
