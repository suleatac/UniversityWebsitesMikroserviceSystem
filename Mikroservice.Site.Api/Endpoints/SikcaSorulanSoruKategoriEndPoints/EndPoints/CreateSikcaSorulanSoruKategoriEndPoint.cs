using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.SikcaSorulanSoruKategoriFeatures.CreateSikcaSorulanSoruKategori;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruKategoriEndPoints.EndPoints
{
    public static class CreateSikcaSorulanSoruKategoriEndPoint
    {
        public static RouteGroupBuilder CreateSikcaSorulanSoruKategoriEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateSikcaSorulanSoruKategoriCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateSikcaSorulanSoruKategori")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateSikcaSorulanSoruKategoriCommand>>();

            return group;
        }
    }
}
