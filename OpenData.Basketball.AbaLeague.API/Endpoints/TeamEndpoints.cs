using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basketball.AbaLeague.API.Filters;
using OpenData.Basketball.AbaLeague.Application.DTOs.Season;
using OpenData.Basketball.AbaLeague.Application.Features.Seasons.Queries.GetSeasons;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamById;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeams;

namespace OpenData.Basketball.AbaLeague.API.Endpoints
{
    public static class TeamEndpoints
    {
        public static WebApplication MapTeamEndpoints(this WebApplication app)
        {
            var root = app.MapGroup("/api/team")
           .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
           .WithTags("Basketball.Team")
           .WithDescription("Lookup and find team")
           .WithOpenApi();

            _ = root.MapGet("/all", (Delegate) GetTeams)
                    .Produces<List<SeasonItemDto>>()
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithName("GetTeams")
                    .RequireAuthorization();

            _ = root.MapGet("/", (Delegate) GetTeamById)
                    .Produces<List<SeasonItemDto>>()
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithName("GetTeamById")
                    .RequireAuthorization();

            return app;
        }

        public static async Task<IResult> GetTeams([FromServices] IMediator mediator, 
                                                    CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new GetTeamsQuery("", 1, 200), cancellationToken);
                if (result.HasValue)
                {
                    return Results.Ok(result.Value.Teams);
                }
                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public static async Task<IResult> GetTeamById([FromServices] IMediator mediator,
                                                        [FromQuery] int teamId,
                                                        CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new GetTeamByIdQuery(teamId), cancellationToken);
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

