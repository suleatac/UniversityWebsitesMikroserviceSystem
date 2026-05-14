using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mikroservice.Site.Application.Features.BannerFeatures.ReorderBanners;
using Microservice.Shared.Extentions;

namespace Mikroservice.Site.Api.Endpoints.BannerEndPoints.EndPoints
{
    public static class ReorderBannersEndPoint
    {
        public static RouteGroupBuilder ReorderBannersEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPut("/reorder", async (
                [FromBody] ReorderBannersCommand command,
                [FromServices] IMediator mediator) =>
            {
                var result = await mediator.Send(command);
                return result.ToGenericResult();
            })
            .WithName("ReorderBanners")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

            return group;
        }
    }
}
