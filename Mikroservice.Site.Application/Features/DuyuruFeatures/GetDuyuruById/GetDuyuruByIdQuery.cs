using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.DuyuruDtos;

namespace Mikroservice.Site.Application.Features.DuyuruFeatures.GetDuyuruById
{
    public record GetDuyuruByIdQuery(int Id) : IRequestByServiceResult<DuyuruDetailDto>;
}