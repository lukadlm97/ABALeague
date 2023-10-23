using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OpenData.Basketball.AbaLeague.Application.Contracts.Leagues;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagueById;

namespace OpenData.Basketball.AbaLeague.WebApp.Pages.Leagues
{
    public class DetailsModel:PageModel
    {
        private readonly ISender _sender;

        public DetailsModel(ISender sender)
        {
            _sender = sender;
        }

        public LeagueResponse League { get; set; }
        public bool IsLoaded { get; set; } = false;
        public bool IsError { get; set; } = false;
        public string ErrorMessage { get; set; }

        public async Task OnGetAsync(int? leagueId, CancellationToken cancellationToken = default)
        {
            if (leagueId == null)
            {
                return;
            }
            IsLoaded = false;
            IsError = false;
            var league = await _sender.Send(new GetLeagueByIdQuery(leagueId??0), cancellationToken);
            if (league.HasNoValue)
            {
                IsLoaded = true;
                IsError = true;
                ErrorMessage = "Cant retrieve selected league";
            }

            IsLoaded = true;
            League = league.Value;
        }
    }
}
