using OpenData.Basketball.AbaLeague.API.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using OpenData.Basketball.AbaLeague.Application.Features.Seasons.Queries.GetSeasons;
using OpenData.Basketball.AbaLeague.Application.DTOs.Season;
using OpenData.Basketball.AbaLeague.Application.Features.Seasons.Queries.GetLeaguesBySeasonId;

namespace OpenData.Basketball.AbaLeague.API.Endpoints
{
    public static class SeasonEndpoints
    {
        public static WebApplication MapSeasonEndpoints(this WebApplication app)
        {
            var root = app.MapGroup("/api/season")
           .AddEndpointFilterFactory(ValidationFilter.ValidationFilterFactory)
           .WithTags("Basketball.Season")
           .WithDescription("Lookup and find season")
           .WithOpenApi();

            _ = root.MapGet("/all", (Delegate) GetSeasons)
                    .Produces<List<SeasonItemDto>>()
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithName("GetSeasons")
                    .RequireAuthorization(); 

            _ = root.MapGet("/", (Delegate) SearchLeaguesBySeasonId)
                    .Produces(StatusCodes.Status200OK)
                    .ProducesProblem(StatusCodes.Status404NotFound)
                    .ProducesProblem(StatusCodes.Status500InternalServerError)
                    .WithName("SearchLeaguesBySeasonId")
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
                   return Results.Ok(result.Value.SeasonItems);
                }
                return Results.NotFound();
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
        public static async Task<IResult> SearchLeaguesBySeasonId([FromServices] IMediator mediator,
                                                                    [FromQuery] int seasonId,
                                                                    CancellationToken cancellationToken = default)
        {
            try
            {
                var seasons = await mediator.Send(new GetSeasonsQuery());
                if (seasons.HasNoValue || !seasons.Value.SeasonItems.Any(x=>x.Id == seasonId))
                {
                    return Results.NotFound();
                }


                var leagues = await mediator.Send(new GetLeaguesBySeasonIdQuery(seasonId), cancellationToken);

                if (leagues.HasNoValue)
                {
                    return Results.Ok(new
                    {
                        Season = seasons.Value.SeasonItems.First(x=>x.Id == seasonId),
                    });
                }
                return Results.Ok(new
                {
                    Season = seasons.Value.SeasonItems.First(x => x.Id == seasonId),
                    Leagues = leagues.Value.LeagueItems
                });
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
