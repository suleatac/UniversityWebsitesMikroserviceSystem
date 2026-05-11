using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.UnvanDtos;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.GetUnvans
{
    public record GetUnvanQuery : IRequestByServiceResult<List<UnvanDto>>;

}
