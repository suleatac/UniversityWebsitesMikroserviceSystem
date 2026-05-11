using MediatR;
using Mikroservice.Site.Domain.Entities;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.GetSikcaSorulanSoruKategoriById;

namespace Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruKategoriEndPoints.EndPoints
{
    public static class GetSikcaSorulanSoruKategoriByIdEndPoint
    {
        public static RouteGroupBuilder GetSikcaSorulanSoruKategoriByIdEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
            {
                var result = await mediator.Send(new GetSikcaSorulanSoruKategoriByIdQuery(id));
                return result.ToGenericResult();
            })
            .WithName("GetSikcaSorulanSoruKategoriById")
            .MapToApiVersion(1.0)
            .Produces<SikcaSorulanSoruKategori>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}