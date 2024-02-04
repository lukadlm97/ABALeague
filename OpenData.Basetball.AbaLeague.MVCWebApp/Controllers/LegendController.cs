using MediatR;
using Microsoft.AspNetCore.Mvc;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basetball.AbaLeague.MVCWebApp.Models;
using OpenData.Basetball.AbaLeague.MVCWebApp.Utilities;
using OpenData.Basketball.AbaLeague.Application.Features.Leagues.Queries.GetLeagues;

namespace OpenData.Basetball.AbaLeague.MVCWebApp.Controllers
{
    public class LegendController : Controller
    {
        private readonly ILogger<LeagueController> _logger;
        private readonly ISender _sender;

        public LegendController(ILogger<LeagueController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
        {
            List<LegendTermViewModel> terms = new List<LegendTermViewModel>();  
            List<LegendItemViewModel> list = new List<LegendItemViewModel>();

            foreach(var item in Enum.GetValues(typeof(PositionEnum)).Cast<PositionEnum>())
            {
                list.Add(new LegendItemViewModel()
                {
                    Color = item.ConvertPositionEnumToColor(),
                    Name = item.ToString(),
                });
            }
            terms.Add(new LegendTermViewModel()
            {
                Title = "Position",
                LegendItems = list,
            });

            return View(new LegendViewModel()
            {
                Terms = terms
            });
        }

    }
}
