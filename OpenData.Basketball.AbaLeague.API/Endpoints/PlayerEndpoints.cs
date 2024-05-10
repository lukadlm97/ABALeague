using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basketball.AbaLeague.API.Filters;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayerById;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayers;
using OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetRosterHistoryByPlayerId;

namespace OpenData.Basketball.AbaLeague.API.Endpoints
{
    public static class PlayerEndpoints
    {
        public static WebApplication MapPlayerEndpoints(this WebApplication app)
        {
            var root = app.MapGroup("/api/player")
           .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
           .WithTags("Basketball.Player")
           .WithDescription("Lookup and find players")
           .WithOpenApi();

            _ = root.MapGet("/all", (Delegate) GetPlayers)
                    .Produces<List<LeagueItemDto>>()
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithName("GetLeagues")
                    .RequireAuthorization();

            _ = root.MapGet("/", (Delegate) GetPlayerById)
                   .Produces(StatusCodes.Status200OK)
                   .ProducesProblem(StatusCodes.Status404NotFound)
                   .ProducesProblem(StatusCodes.Status500InternalServerError)
                   .WithName("GetLeagueById")
                   .RequireAuthorization();

            _ = root.MapGet("/rosterHistory", (Delegate) GetPlayerRosterHistoryById)
                   .Produces(StatusCodes.Status200OK)
                   .ProducesProblem(StatusCodes.Status404NotFound)
                   .ProducesProblem(StatusCodes.Status500InternalServerError)
                   .WithName("GetPlayerRosterHistoryById")
                   .RequireAuthorization();

            return app;
        }

        public static async Task<IResult> GetPlayers([FromServices] IMediator mediator,
                                                        CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new GetPlayersQuery(1, 1000));
                if (result.HasValue)
                {
                    return Results.Ok(result.Value.Players);
                }
                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        } 
        public static async Task<IResult> GetPlayerById([FromServices] IMediator mediator,
                                                        [FromQuery] int playerId,
                                                        CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new GetPlayerByIdQuery(playerId));
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
        public static async Task<IResult> GetPlayerRosterHistoryById([FromServices] IMediator mediator,
                                                        [FromQuery] int playerId,
                                                        CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new GetRosterHistoryByPlayerIdQuery(playerId));
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
