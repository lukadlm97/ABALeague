using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueById;
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
        public int? LeagueId { get; set; }
        public bool IsLoaded { get; set; } = false;
        public bool IsError { get; set; } = false;
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(int? leagueId, CancellationToken cancellationToken=default)
        {
            if (leagueId == null)
            {
                LeagueId = 0;
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
                Season = league.Value.Season,
                BaseUrl = league.Value.BaseUrl,
                BoxScoreUrl = league.Value.BoxScoreUrl,
                CalendarUrl = league.Value.CalendarUrl,
                MatchUrl = league.Value.MatchUrl,
                OfficalName = league.Value.OfficialName,
                RosterUrl = league.Value.RosterUrl,
                StandingUrl = league.Value.StandingUrl
            };
        }

        public async Task OnPostAsync(CancellationToken cancellationToken = default)
        {
            IsLoaded = false;
            IsError = false;
            if (!ModelState.IsValid)
            {
                IsError = true;
                ErrorMessage = "Invalid model state";
                return;
            }

            try
            {
                IsLoaded = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
