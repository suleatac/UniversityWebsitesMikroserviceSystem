using MediatR;
using Microservice.Shared.Extentions;
using Microservice.Shared.Filters;
using Microservice.Site.Application.Features.YoneticiDuyuruFeatures.CreateYoneticiDuyuru;
using Microsoft.AspNetCore.Mvc;


namespace Microservice.Site.Api.Endpoints.YoneticiDuyuruEndPoints.EndPoints
{
    public static class CreateYoneticiDuyuruEndPoint
    {
        public static RouteGroupBuilder CreateYoneticiDuyuruEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async ([FromServices] IMediator mediator, [FromBody] CreateYoneticiDuyuruCommand command) => {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
             .WithName("CreateYoneticiDuyuru")
             .MapToApiVersion(1.0)
             .Produces<Guid>(StatusCodes.Status201Created)
             .Produces(StatusCodes.Status400BadRequest)
             .Produces(StatusCodes.Status404NotFound)
             .Produces(StatusCodes.Status500InternalServerError)
             .AddEndpointFilter<ValidationFilter<CreateYoneticiDuyuruCommand>>();
            return group;
        }
    }
}
