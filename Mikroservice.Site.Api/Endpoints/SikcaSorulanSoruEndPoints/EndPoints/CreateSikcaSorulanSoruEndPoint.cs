using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.SikcaSorulanSoruFeatures.CreateSikcaSorulanSoru;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.SikcaSorulanSoruEndPoints.EndPoints
{
    public static class CreateSikcaSorulanSoruEndPoint
    {
        public static RouteGroupBuilder CreateSikcaSorulanSoruEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateSikcaSorulanSoruCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateSikcaSorulanSoru")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateSikcaSorulanSoruCommand>>();

            return group;
        }
    }
}
