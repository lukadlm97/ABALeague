using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;

namespace OpenData.Basketball.AbaLeague.WebApp.Pages.Leagues
{
    public class IndexModel : PageModel
    {
        private readonly ISender _sender;

        public IndexModel(ISender sender)
        {
            _sender = sender;
        }

        public IList<LeagueItemDto> Leagues { get; set; }
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; } = "There is no leagues to display.";

        public async Task OnGetAsync(CancellationToken cancellationToken = default)
        {
            var response = await _sender.Send(new GetLeagueQuery(), cancellationToken);
            if (response.HasNoValue)
            {
                IsSuccess = false;
                return;
            }

            IsSuccess = true;
            Leagues = response.Value.LeagueResponses.ToList();
        }
    }
}
