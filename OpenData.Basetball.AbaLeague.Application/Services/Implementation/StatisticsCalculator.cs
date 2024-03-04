using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.DTOs.Statistic;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Services.Implementation
{
    public class StatisticsCalculator : IStatisticsCalculator
    {
        public FrozenSet<CategoriesItemDto> 
            Calcualate(IEnumerable<BoxScore> boxScores,
                        FrozenSet<PositionEnum> positions)
        {
            List<CategoriesItemDto> categoriesItems = new List<CategoriesItemDto>();
            foreach (PositionEnum position in positions)
            {
                var selectedPerformancesByPosition = boxScores
                                            .Where(x => x.RosterItem.Player.PositionEnum == position);
                if (categoriesItems.Select(x => x.PositionEnum).Contains(position))
                {
                    throw new NotImplementedException();
                }
                var evaluated = selectedPerformancesByPosition.ToList();
                categoriesItems.Add(new CategoriesItemDto(position,
                  new PerformanceItem(
                    selectedPerformancesByPosition.Sum(x => x.Points) ?? 0,
                    Math.Round((decimal)selectedPerformancesByPosition.Sum(x => x.Points)*100 / boxScores.Sum(x=>x.Points) ?? 0, 2),
                    selectedPerformancesByPosition.Sum(x => x.TotalRebounds) ?? 0,
                    Math.Round((decimal)selectedPerformancesByPosition.Sum(x => x.TotalRebounds) * 100 / boxScores.Sum(x => x.TotalRebounds) ?? 0, 2),
                    selectedPerformancesByPosition.Sum(x => x.Assists) ?? 0,
                    Math.Round((decimal)selectedPerformancesByPosition.Sum(x => x.Assists) * 100 / boxScores.Sum(x => x.Assists) ?? 0, 2),
                    selectedPerformancesByPosition.Sum(x => x.Turnover) ?? 0,
                    Math.Round((decimal)selectedPerformancesByPosition.Sum(x => x.Turnover) * 100 / boxScores.Sum(x => x.Turnover) ?? 0, 2) ,
                    selectedPerformancesByPosition.Sum(x => x.Steals) ?? 0,
                    Math.Round((decimal)selectedPerformancesByPosition.Sum(x => x.Steals) * 100 / boxScores.Sum(x => x.Steals) ?? 0, 2),
                    selectedPerformancesByPosition.Sum(x => x.OffensiveRebounds) ?? 0,
                    Math.Round((decimal)selectedPerformancesByPosition.Sum(x => x.OffensiveRebounds) * 100 / boxScores.Sum(x => x.OffensiveRebounds) ?? 0, 2) ,
                    selectedPerformancesByPosition.Sum(x => x.DefensiveRebounds) ?? 0,
                    Math.Round((decimal)selectedPerformancesByPosition.Sum(x => x.DefensiveRebounds) * 100 / boxScores.Sum(x => x.DefensiveRebounds) ?? 0, 2),
                    selectedPerformancesByPosition.Sum(x => x.InFavoureOfBlock) ?? 0,
                    Math.Round((decimal)selectedPerformancesByPosition.Sum(x => x.InFavoureOfBlock) * 100 / boxScores.Sum(x => x.InFavoureOfBlock) ?? 0, 2) ,
                    selectedPerformancesByPosition.Sum(x => x.AgainstBlock) ?? 0,
                    Math.Round((decimal?)selectedPerformancesByPosition.Sum(x => x.AgainstBlock) * 100 / boxScores.Sum(x => x.AgainstBlock) ?? 0, 2)
                )));
            }

            return categoriesItems.ToFrozenSet();
        }

        public FrozenSet<CategoriesItemByMatchDto> CalcualateByMatch(IEnumerable<BoxScore> boxScores,
                                                                        FrozenSet<PositionEnum> positions)
        {
            var matchIds = boxScores.Select(x => x.RoundMatchId)
                                    .Distinct()
                                    .ToFrozenSet();
            List<CategoriesItemByMatchDto> list = new List<CategoriesItemByMatchDto>();
            foreach(var matchId in matchIds)
            {
                var subsetOfBoxscore = boxScores.Where(x=>x.RoundMatchId == matchId);
                var categoriesByMatch = Calcualate(subsetOfBoxscore, positions);
                foreach(var position in positions)
                {
                    var newItem = categoriesByMatch.FirstOrDefault(x => x.PositionEnum == position);
                    if(newItem == null)
                    {
                        throw new NotImplementedException();
                    }
                    list.Add(new CategoriesItemByMatchDto(matchId, position, newItem.BoxScoreItemDto));
                }
            }

            return list.ToFrozenSet();
        }
    }
}
