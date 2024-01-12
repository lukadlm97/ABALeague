using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Commands.CreateLeague;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Commands.UpdateLeague;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueById;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.WebApp.Models;

namespace OpenData.Basketball.AbaLeague.WebApp.Pages.Leagues
{
    public class CreateModel : PageModel
    {
        private readonly ISender _sender;

        public CreateModel(ISender sender)
        {
            _sender = sender;
        }
        [BindProperty] public League League { get; set; } = default!;
        public bool IsLoaded { get; set; } = true;
        public bool IsError { get; set; } = false;
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(int? leagueId, CancellationToken cancellationToken=default)
        {
            TempData["LeagueId"] = leagueId;
            if (leagueId == null)
            {
                return;
            }
            IsLoaded = false;
            IsError = false;

            var league = await _sender.Send(new GetLeagueByIdQuery(leagueId ?? 0), cancellationToken);
            if (league.HasNoValue)
            {
                IsLoaded = true;
                IsError = true;
                ErrorMessage = "Cant retrieve selected league";

                return;
            }
            IsLoaded = true;
            League = new League()
            {
                ShortName = league.Value.ShortName,
                BaseUrl = league.Value.BaseUrl,
                BoxScoreUrl = league.Value.BoxScoreUrl,
                CalendarUrl = league.Value.CalendarUrl,
                MatchUrl = league.Value.MatchUrl,
                OfficalName = league.Value.OfficialName,
                RosterUrl = league.Value.RosterUrl,
                StandingUrl = league.Value.StandingUrl
            };
        }
        /*
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
        {
            IsLoaded = false;
            IsError = false;
            if (!ModelState.IsValid)
            {
                IsLoaded = true;
                return Page();
            }

            var leagueId = (int?) TempData["LeagueId"];
            Result? result = null;
            try
            {
                if (leagueId == null)
                {
                    result = await _sender.Send(new CreateLeagueCommand(League.OfficalName, League.ShortName,
                      League.StandingUrl, League.CalendarUrl, League.MatchUrl, League.BoxScoreUrl,
                        League.BaseUrl, League.RosterUrl, 1, 1), cancellationToken);
                   
                }
                else
                {
                    result = await _sender.Send(new UpdateLeagueCommand(leagueId ?? 0, League.OfficalName,
                        League.ShortName,
                        League.Season, League.StandingUrl, League.CalendarUrl, League.MatchUrl, League.BoxScoreUrl,
                        League.BaseUrl, League.RosterUrl), cancellationToken);
                  
                }
                if (result.IsSuccess)
                {
                    return RedirectToPage("/Leagues/Index");
                }
                else
                {
                    TempData["ErrorMessage"] = result.Error.Message;
                    return RedirectToPage("/Leagues/Error");
                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
                return RedirectToPage("/Leagues/Error");
            }
        }
        */
    }
}
