using OpenData.Basketball.AbaLeague.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using OpenData.Basketball.AbaLeague.Application.Features.Seasons.Queries.GetSeasons;

namespace OpenData.Basketball.AbaLeague.API.Endpoints
{
    public static class Season
    {
        public static WebApplication MapSeasonEndpoints(this WebApplication app)
        {
            var root = app.MapGroup("/api/season")
           .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
           .WithTags("Season")
           .WithDescription("Lookup and Find season")
           .WithOpenApi();

            _ = root.MapGet("/all", GetSeasons)
                    .Produces(StatusCodes.Status200OK)
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithName("GetSeasons")
                    .RequireAuthorization();

            return app;
        }

        public static async Task<IResult> GetSeasons([FromServices] IMediator mediator)
        {
            try
            {
                var result = await mediator.Send(new GetSeasonsQuery());
                if (result.HasValue)
                {
                   return Results.Ok(result.Value);
                }
                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
