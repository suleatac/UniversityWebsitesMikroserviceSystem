using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.BandLogoDtos;

namespace Mikroservice.Site.Application.Features.BandLogoFeatures.GetBandLogoById
{
    public record GetBandLogoByIdQuery(int Id) : IRequestByServiceResult<BandLogoDetailDto>;
}
