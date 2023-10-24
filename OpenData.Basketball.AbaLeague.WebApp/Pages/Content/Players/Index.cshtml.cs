using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;
using OpenData.Basketball.AbaLeague.WebApp.Models;

namespace OpenData.Basketball.AbaLeague.WebApp.Pages.Content.Players
{
    public class IndexModel:PageModel
    {
        private readonly ISender _sender;

        public IndexModel(ISender sender)
        {
            _sender = sender;
        }

        public IList<LeagueResponse> Leagues { get; set; }
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; } = "There is no leagues to display.";


        public async Task OnGetAsync(CancellationToken cancellationToken = default)
        {
            var leagues = await _sender.Send(new GetLeagueQuery(), cancellationToken);
            if (leagues.HasNoValue)
            {
                IsSuccess = false;
                return;
            }

            Leagues = leagues.Value.LeagueResponses.ToList();
        }

        public async Task OnPostAsync(CancellationToken cancellationToken = default)
        {
        }
    }
}
