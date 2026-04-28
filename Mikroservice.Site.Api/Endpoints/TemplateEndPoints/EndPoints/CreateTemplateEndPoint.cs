using MediatR;
using Microservice.Shared.Filters;
using Microsoft.AspNetCore.Mvc;
using Microservice.Shared.Extentions;
using Mikroservice.Site.Application.Features.TemplateFeatures.CreateTemplate;

namespace Mikroservice.Site.Api.Endpoints.TemplateEndPoints.EndPoints
{
    public static class CreateTemplateEndPoint
    {
        
        public static RouteGroupBuilder CreateTemplateEndpointGroupItem(this RouteGroupBuilder group)
        {
            group.MapPost("/", async (
                [FromServices] IMediator mediator,
                [FromBody] CreateTemplateCommand command) => {
                    var result = await mediator.Send(command);
                    return result.ToGenericResult();
                })
            .WithName("CreateTemplate")
            .MapToApiVersion(1.0)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .AddEndpointFilter<ValidationFilter<CreateTemplateCommand>>();

            return group;
        }
    
}
}
