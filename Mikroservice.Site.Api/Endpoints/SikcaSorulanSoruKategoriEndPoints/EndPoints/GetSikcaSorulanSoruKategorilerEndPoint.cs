using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using MediatR;
using Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.GetSikcaSorulanSoruKategori;

namespace Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruKategoriEndPoints.EndPoints
{
    public static class GetSikcaSorulanSoruKategorilerEndPoint
    {
        public static RouteGroupBuilder GetSikcaSorulanSoruKategorilerEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/", async (IMediator mediator) =>
            {
                var result = await mediator.Send(new GetSikcaSorulanSoruKategoriQuery());
                return result.ToGenericResult();
            })
            .WithName("GetSikcaSorulanSoruKategoriler")
            .MapToApiVersion(1.0)
            .Produces<List<SikcaSorulanSoruKategori>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
