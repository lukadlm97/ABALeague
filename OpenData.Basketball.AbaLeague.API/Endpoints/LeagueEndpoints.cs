using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basketball.AbaLeague.API.Filters;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.DTOs.Season;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueById;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetRegistredTeamsByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetStandingsByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Schedules.Queries.GetScheduleByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Score.Queries.GetScoresByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.Seasons.Queries.GetSeasons;

namespace OpenData.Basketball.AbaLeague.API.Endpoints
{
    public static class LeagueEndpoints
    {
        public static WebApplication MapLeagueEndpoints(this WebApplication app)
        {
            var root = app.MapGroup("/api/league")
           .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
           .WithTags("Basketball.League")
           .WithDescription("Lookup and find leagues")
           .WithOpenApi();

            _ = root.MapGet("/all", (Delegate) GetLeagues)
                    .Produces<List<LeagueItemDto>>()
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithName("GetLeagues")
                    .RequireAuthorization();

            _ = root.MapGet("/", (Delegate) GetLeagueById)
                   .Produces(StatusCodes.Status200OK)
                   .ProducesProblem(StatusCodes.Status404NotFound)
                   .ProducesProblem(StatusCodes.Status500InternalServerError)
                   .WithName("GetLeagueById")
                   .RequireAuthorization();

            _ = root.MapGet("/standings", (Delegate) GetLeagueStandingsById)
                   .Produces<StandingsDto>()
                   .ProducesProblem(StatusCodes.Status404NotFound)
                   .ProducesProblem(StatusCodes.Status500InternalServerError)
                   .WithName("GetLeagueStandingsById")
                   .RequireAuthorization();

            _ = root.MapGet("/scheduled", (Delegate) GetLeagueScheduledMatchesById)
                   .Produces<StandingsDto>()
                   .ProducesProblem(StatusCodes.Status404NotFound)
                   .ProducesProblem(StatusCodes.Status500InternalServerError)
                   .WithName("GetLeagueScheduledMatchesById")
                   .RequireAuthorization(); 
            
            _ = root.MapGet("/scores", (Delegate) GetLeagueResultMatchesById)
                   .Produces<StandingsDto>()
                   .ProducesProblem(StatusCodes.Status404NotFound)
                   .ProducesProblem(StatusCodes.Status500InternalServerError)
                   .WithName("GetLeagueResultMatchesById")
                   .RequireAuthorization();

            return app;
        }

        public static async Task<IResult> GetLeagues([FromServices] IMediator mediator, 
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
        public static async Task<IResult> GetLeagueById([FromQuery] int leagueId, 
                                                    [FromServices] IMediator mediator,
                                                    CancellationToken cancellationToken = default)
        {
            try
            {
                var league = await mediator.Send(new GetLeagueByIdQuery(leagueId));
                if (league.HasNoValue)
                {
                    return Results.NotFound();
                }
                var registredTeams = await mediator.Send(new GetRegistredTeamsByLeagueIdQuery(leagueId), cancellationToken);

                if(registredTeams.HasNoValue)
                {
                    return Results.Ok(new
                    {
                        League = league.Value
                    });
                }

                return Results.Ok(new
                {
                    League = league.Value,
                    RegistredTeams = registredTeams.Value.Teams
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
        public static async Task<IResult> GetLeagueStandingsById([FromQuery] int leagueId,
                                                    [FromServices] IMediator mediator,
                                                    CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await mediator.Send(new GetStandingsByLeagueIdQuery(leagueId));
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
        public static async Task<IResult> GetLeagueScheduledMatchesById([FromQuery] int leagueId,
                                                    [FromServices] IMediator mediator,
                                                    CancellationToken cancellationToken = default)
        {
            try
            {
                var scheduledMatches = await mediator.Send(new GetScheduleByLeagueIdQuery(leagueId));
                if (scheduledMatches.HasValue)
                {
                    return Results.Ok(scheduledMatches.Value.ExistingScheduleItems.Items.OrderBy(x=>x.Round));
                }
                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }  
        public static async Task<IResult> GetLeagueResultMatchesById([FromQuery] int leagueId,
                                                    [FromServices] IMediator mediator,
                                                    CancellationToken cancellationToken = default)
        {
            try
            {
                var matchesResult = await mediator.Send(new GetScoresByLeagueIdQuery(leagueId));
                if (matchesResult.HasValue)
                {
                    return Results.Ok(matchesResult.Value.ScoreItems.OrderBy(x=>x.Round));
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
