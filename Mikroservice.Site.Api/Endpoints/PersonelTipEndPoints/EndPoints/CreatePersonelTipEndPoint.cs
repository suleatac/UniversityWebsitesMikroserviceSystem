using Microservice.Shared.Filters;
using Mikroservice.Site.Application.Features.PersonelTipFeatures.CreatePersonelTip;
using Microservice.Shared.Extentions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mikroservice.Site.Api.Endpoints.PersonelTipEndPoints.EndPoints
{
    public static class CreatePersonelTipEndPoint
    {
        public static RouteGroupBuilder CreatePersonelTipEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreatePersonelTipCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreatePersonelTip")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreatePersonelTipCommand>>();

            return group;
        }
    }
}
