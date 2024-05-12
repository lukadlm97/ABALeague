using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basketball.AbaLeague.API.Filters;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreByMatchResultId;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using OpenData.Basketball.AbaLeague.Application.Features.Matches.Queries.GetByMatchId;
using OpenData.Basketball.AbaLeague.Application.Features.Matches.Queries.GetMatchesByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoreById;
using OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoresByRoundAndLeagueId;

namespace OpenData.Basketball.AbaLeague.API.Endpoints
{
    public static class GameEndpoint
    {
        public static WebApplication MapGameEndpoints(this WebApplication app)
        {
            var root = app.MapGroup("/api/game")
           .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
           .WithTags("Basketball.Game")
           .WithDescription("Lookup and find games")
           .WithOpenApi();

            _ = root.MapGet("/all", (Delegate) GetGames)
                    .Produces<List<LeagueItemDto>>()
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithName("GetGames")
                    .RequireAuthorization();

            _ = root.MapGet("/", (Delegate) GetGamesById)
                   .Produces(StatusCodes.Status200OK)
                   .ProducesProblem(StatusCodes.Status404NotFound)
                   .ProducesProblem(StatusCodes.Status500InternalServerError)
                   .WithName("GetGamesById")
                   .RequireAuthorization(); 
            
            _ = root.MapGet("/byLeague", (Delegate) GetGamesByLeagueId)
                   .Produces(StatusCodes.Status200OK)
                   .ProducesProblem(StatusCodes.Status404NotFound)
                   .ProducesProblem(StatusCodes.Status500InternalServerError)
                   .WithName("GetGamesByLeagueId")
                   .RequireAuthorization();

            _ = root.MapGet("/result", (Delegate) GetGameResultById)
                  .Produces(StatusCodes.Status200OK)
                  .ProducesProblem(StatusCodes.Status404NotFound)
                  .ProducesProblem(StatusCodes.Status500InternalServerError)
                  .WithName("GetGameResultById")
                  .RequireAuthorization();          
            
            _ = root.MapGet("/boxscore", (Delegate) GetGameBoxscoreById)
                  .Produces(StatusCodes.Status200OK)
                  .ProducesProblem(StatusCodes.Status404NotFound)
                  .ProducesProblem(StatusCodes.Status500InternalServerError)
                  .WithName("GetGameBoxscoreById")
                  .RequireAuthorization();  
            
            _ = root.MapGet("/byRound/results", (Delegate) GetGameResultsByRoundAndLeagueId)
                  .Produces(StatusCodes.Status200OK)
                  .ProducesProblem(StatusCodes.Status404NotFound)
                  .ProducesProblem(StatusCodes.Status500InternalServerError)
                  .WithName("GetGameResultsByRoundAndLeagueId")
                  .RequireAuthorization();



            return app;
        }

        public static async Task<IResult> GetGames([FromServices] IMediator mediator,
                                                        CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new GetLeagueQuery());
                if (result.HasValue)
                {
                    return Results.Ok(result.Value.LeagueItems);
                }
                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        } 
        public static async Task<IResult> GetGamesById([FromServices] IMediator mediator,
                                                        [FromQuery] int gameId,
                                                        CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new GetByMatchIdQuery(gameId));
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
        public static async Task<IResult> GetGameResultById([FromServices] IMediator mediator,
                                                        [FromQuery] int gameId,
                                                        CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new GetScoreByIdQuery(gameId), cancellationToken);
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
        
        public static async Task<IResult> GetGamesByLeagueId([FromServices] IMediator mediator,
                                                        [FromQuery] int leagueId,
                                                        CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new GetMatchesByLeagueIdQuery(leagueId), cancellationToken);
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
     
        public static async Task<IResult> GetGameBoxscoreById([FromServices] IMediator mediator,
                                                        [FromQuery] int gameId,
                                                        CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new GetBoxscoreByMatchResultIdQuery(gameId), cancellationToken);
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

        public static async Task<IResult> GetGameResultsByRoundAndLeagueId([FromServices] IMediator mediator,
                                                     [FromQuery] int leagueId,
                                                     [FromQuery] int round,
                                                     CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new GetScoresByRoundAndLeagueIdQuery(round, leagueId), cancellationToken);
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
