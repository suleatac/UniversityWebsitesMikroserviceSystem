using Microservice.Shared;
using Mikroservice.Site.Application.DTOs.SikcaSorulanSoruDtos;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.GetSikcaSorulanSoruById
{
    public record GetSikcaSorulanSoruByIdQuery(int Id) : IRequestByServiceResult<SikcaSorulanSoruDetailDto>;
}