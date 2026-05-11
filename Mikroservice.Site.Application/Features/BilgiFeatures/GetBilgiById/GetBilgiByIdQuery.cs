using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.BilgiDtos;

namespace Mikroservice.Site.Application.Features.BilgiFeatures.GetBilgiById
{
    public record GetBilgiByIdQuery(int Id) : IRequestByServiceResult<BilgiDetailDto>;
}