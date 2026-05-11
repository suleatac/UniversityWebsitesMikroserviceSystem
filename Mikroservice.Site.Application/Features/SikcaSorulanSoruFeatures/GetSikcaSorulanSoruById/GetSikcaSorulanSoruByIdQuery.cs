using Microservice.Shared;
using Mikroservice.Site.Domain.Entities;

namespace Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.GetSikcaSorulanSoruById
{
    public record GetSikcaSorulanSoruByIdQuery(int Id) : IRequestByServiceResult<SikcaSorulanSoru>;
}