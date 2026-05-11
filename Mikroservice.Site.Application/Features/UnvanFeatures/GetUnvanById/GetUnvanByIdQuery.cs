using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.UnvanDtos;

namespace Mikroservice.Site.Application.Features.UnvanFeatures.GetUnvanById
{
    public record GetUnvanByIdQuery(int Id) : IRequestByServiceResult<UnvanDetailDto>;
}
