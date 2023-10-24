using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetTeamsByLeagueId;
using OpenData.Basketball.AbaLeague.Application.Features.SeasonResources.Commands.CreateSeasonResources;
using OpenData.Basketball.AbaLeague.Domain.Enums;

namespace OpenData.Basketball.AbaLeague.WebApp.Pages.Content.Teams
{
    public class DraftModel : PageModel
    {
        private readonly ISender _sender;

        public DraftModel(ISender sender)
        {
            _sender = sender;
        }
        public bool IsSuccess { get; set; } = false;
        public string ErrorMessage { get; set; } = "There is no leagues to display.";
        public IList<TeamDTO> Teams { get; set; }
        public async Task OnGetAsync(int? leagueId, CancellationToken cancellationToken = default)
        {
            TempData["LeagueId"] = leagueId;
            var teams = await _sender.Send(new GetTeamsByLeagueIdQuery(leagueId ?? 0), cancellationToken);
            if (teams.HasNoValue)
            {
                IsSuccess = false;
                return;
            }
            Teams = teams.Value.ToList();
            var teamsJson = JsonConvert.SerializeObject(teams.Value.ToList());
            TempData["Teams"] = teamsJson;

        }

        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
        {
            var leagueId = (int?)TempData["LeagueId"];

            try
            {
                var teamsJson = TempData["Teams"] as string;
                var teams = JsonConvert.DeserializeObject<List<TeamDTO>>(teamsJson);
                if (leagueId == null || teams == null)
                {
                    return Page();
                }

                teams = teams.Where(x => x.MaterializationStatus == MaterializationStatus.NotExist).ToList();
                
                var result = await _sender.Send(new CreateSeasonResourcesCommand(teams.Select(x =>
                    new AddSeasonResourceDto(x.TeamId ?? 0, leagueId ?? 0, x.Url, x.Name, null, null))), cancellationToken);

                if (result.IsSuccess)
                {
                    return RedirectToPage("/Teams/Index");
                }
                else
                {
                    return RedirectToPage("/League/Error");
                }
            }
            catch (Exception ex)
            {
                return RedirectToPage("/League/Error");
            }
        }
    }
}
