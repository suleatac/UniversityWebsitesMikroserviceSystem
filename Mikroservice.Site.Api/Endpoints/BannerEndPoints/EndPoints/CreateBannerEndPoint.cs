using MediatR;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.BannerFeatures.CreateBanner;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.BannerEndPoints.EndPoints
{
    public static class CreateBannerEndPoint
    {
        public static RouteGroupBuilder CreateBannerEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateBannerCommand command) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("CreateBanner")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateBannerCommand>>();

            return group;
        }
    }
}
