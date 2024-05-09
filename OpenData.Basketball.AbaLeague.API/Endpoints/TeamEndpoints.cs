using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basketball.AbaLeague.API.Filters;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Application.DTOs.Season;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueById;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetAvailableRosterHistoryByTeamId;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterByTeamId;
using OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetRosterPerPositionByTeamAndLeague;
using OpenData.Basketball.AbaLeague.Application.Features.Seasons.Queries.GetSeasons;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamById;
using OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamGamesByLeagueId;
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
                    .Produces(StatusCodes.Status200OK)
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithName("GetTeamById")
                    .RequireAuthorization();

            _ = root.MapGet("/performanceByLeague", (Delegate) GetTeamByLeagueId)
                    .Produces(StatusCodes.Status200OK)
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithName("GetTeamByLeagueId")
                    .RequireAuthorization();

            _ = root.MapGet("/roster", (Delegate) GetTeamRosterById)
                    .Produces(StatusCodes.Status200OK)
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithName("GetTeamRosterById")
                    .RequireAuthorization();

            _ = root.MapGet("/rosterHistory", (Delegate) GetTeamRosterHistoryById)
                   .Produces(StatusCodes.Status200OK)
                   .ProducesProblem(StatusCodes.Status404NotFound)
                   .ProducesProblem(StatusCodes.Status500InternalServerError)
                   .WithName("GetTeamRosterHistoryById")
                   .RequireAuthorization();

            _ = root.MapGet("/rosterByPosition", (Delegate) GetLatestTeamRosterByPositionById)
                    .Produces(StatusCodes.Status200OK)
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithName("GetLatestTeamRosterByPositionById")
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

        public static async Task<IResult> GetTeamByLeagueId([FromServices] IMediator mediator,
                                                      [FromQuery] int teamId,
                                                      [FromQuery] int leagueId,
                                                      CancellationToken cancellationToken = default)
        {
            try
            {
                var teamGames = await mediator.Send(new GetTeamGamesByLeagueIdQuery(leagueId, teamId), cancellationToken);
                var teamRoster = await mediator.Send(new GetRosterByLeagueAndTeamIdQuery(teamId, leagueId), cancellationToken);
               
                if (teamGames.HasValue && teamRoster.HasValue)
                {
                    return Results.Ok(new
                    {
                        TeamDetails = teamGames.Value,
                        RosterDetails = teamRoster.Value.Items
                    });
                }

                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public static async Task<IResult> GetTeamRosterById([FromServices] IMediator mediator,
                                                    [FromQuery] int teamId,
                                                   CancellationToken cancellationToken = default)
        {
            try
            {
                var leagueIdsResult =
                    await mediator.Send(new GetAvailableRosterHistoryByTeamIdQuery(teamId), cancellationToken);
                if (leagueIdsResult.HasNoValue ||
                    !leagueIdsResult.Value.LeagueIds.Any())
                {
                    return Results.NotFound();
                }

                var latestAvailableRoster =
                    await mediator.Send(new GetRosterByLeagueAndTeamIdQuery(teamId, 
                                                                    leagueIdsResult.Value.LeagueIds.Max()));
                if (latestAvailableRoster.HasValue)
                {
                    return Results.Ok(latestAvailableRoster.Value.Items);
                }

                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        } 

        public static async Task<IResult> GetTeamRosterHistoryById([FromServices] IMediator mediator,
                                                    [FromQuery] int teamId,
                                                   CancellationToken cancellationToken = default)
        {
            try
            {
                var leagueIdsResult =
                    await mediator.Send(new GetAvailableRosterHistoryByTeamIdQuery(teamId), cancellationToken);
                if (leagueIdsResult.HasNoValue ||
                    !leagueIdsResult.Value.LeagueIds.Any())
                {
                    return Results.NotFound();
                }

                Dictionary<LeagueItemDto,IEnumerable<RosterItemDto>> rosterByLeague =
                    new Dictionary<LeagueItemDto, IEnumerable<RosterItemDto>>();
                foreach(var leagueId in leagueIdsResult.Value.LeagueIds)
                {
                    var roster = await mediator.Send(new GetRosterByLeagueAndTeamIdQuery(teamId, leagueId), cancellationToken);
                    var league = await mediator.Send(new GetLeagueByIdQuery(leagueId), cancellationToken);
                    if(roster.HasNoValue || league.HasNoValue)
                    {
                        continue;
                    }
                    rosterByLeague.Add(league.Value, roster.Value.Items);
                }

                return Results.Ok(rosterByLeague.Select(x=>new { League = x.Key, Roster = x.Value }));
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        public static async Task<IResult> GetLatestTeamRosterByPositionById([FromServices] IMediator mediator,
                                                   [FromQuery] int teamId,
                                                  CancellationToken cancellationToken = default)
        {
            try
            {
                var leagueIdsResult =
                    await mediator.Send(new GetAvailableRosterHistoryByTeamIdQuery(teamId), cancellationToken);
                if (leagueIdsResult.HasNoValue ||
                    !leagueIdsResult.Value.LeagueIds.Any())
                {
                    return Results.NotFound();
                }

                var latestAvailableRosterByPositions =
                    await mediator.Send(new GetRosterPerPositionByTeamAndLeagueQuery(teamId,
                                                                    leagueIdsResult.Value.LeagueIds.Max()));
                if (latestAvailableRosterByPositions.HasValue)
                {
                    return Results.Ok(latestAvailableRosterByPositions.Value);
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

