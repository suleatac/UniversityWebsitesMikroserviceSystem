using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Microservice.Site.Application.Features.YonetimDuyuruFeatures.CreateYonetimDuyuru;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.BandLogoFeatures.CreateBandLogo;


namespace Microservice.Site.Api.Endpoints.YonetimDuyuruEndPoints.EndPoints
{
    public static class CreateYonetimDuyuruEndPoint
    {
        public static RouteGroupBuilder CreateYonetimDuyuruEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateYonetimDuyuruCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateYonetimDuyuru")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateYonetimDuyuruCommand>>();

            return group;
        }
    }
}
