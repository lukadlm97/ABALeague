using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.DTOs.Statistic;
using OpenData.Basketball.AbaLeague.Application.Services.Contracts;
using OpenData.Basketball.AbaLeague.Application.Utilities;
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
            CalcualatePerPosition(IEnumerable<BoxScore> boxScores,
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

        public FrozenSet<CategoriesItemByMatchDto> CalcualateByMatchPerPosition(IEnumerable<BoxScore> boxScores,
                                                                        FrozenSet<PositionEnum> positions)
        {
            var matchIds = boxScores.Select(x => x.RoundMatchId)
                                    .Distinct()
                                    .ToFrozenSet();
            List<CategoriesItemByMatchDto> list = new List<CategoriesItemByMatchDto>();
            foreach(var matchId in matchIds)
            {
                var subsetOfBoxscore = boxScores.Where(x=>x.RoundMatchId == matchId);
                var categoriesByMatch = CalcualatePerPosition(subsetOfBoxscore, positions);
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

        public TotalAndAveragePerformanceDto
            CalcualteTotalAndAveragePerformance(FrozenSet<BoxScore> boxScores)
        {
            if(boxScores == null || !boxScores.Any())
            {
                return new TotalAndAveragePerformanceDto();
            }

            var gamesPlayed = boxScores.Select(x => x.RoundMatchId).Distinct().Count();

            var sumPoints = boxScores.Sum(x => x.Points);
            var avgPoints = sumPoints / gamesPlayed;

            var sumDefensiveRebounds = boxScores.Sum(x => x.DefensiveRebounds);
            var avgDefensiveRebounds = sumDefensiveRebounds / gamesPlayed;   

            var sumOffensiveRebounds = boxScores.Sum(x => x.OffensiveRebounds);
            var avgOffensiveRebounds = sumOffensiveRebounds / gamesPlayed;  
            
            var sumTotalRebounds = boxScores.Sum(x => x.TotalRebounds);
            var avgTotalRebounds = sumTotalRebounds / gamesPlayed;
            
            var sumAssists = boxScores.Sum(x => x.Assists);
            var avgAssists = sumAssists / gamesPlayed;
            
            var sumSteals = boxScores.Sum(x => x.Steals);
            var avgSteals = sumSteals/gamesPlayed;  

            var sumTurnover = boxScores.Sum(x => x.Turnover);
            var avgTurnover = sumTurnover/gamesPlayed;
            
            var sumInFavoureOfBlock = boxScores.Sum(x => x.InFavoureOfBlock);
            var avgInFavoureOfBlock = sumInFavoureOfBlock / gamesPlayed;  

            var sumAgainstBlock = boxScores.Sum(x => x.AgainstBlock);
            var avgAgainstBlock = sumAgainstBlock / gamesPlayed;

            return new TotalAndAveragePerformanceDto(sumPoints, avgPoints, 
                                                        sumTotalRebounds, avgTotalRebounds, 
                                                        sumAssists, avgAssists, 
                                                        sumTurnover, avgTurnover, 
                                                        sumSteals, avgSteals, 
                                                        sumOffensiveRebounds, avgOffensiveRebounds, 
                                                        sumDefensiveRebounds, avgDefensiveRebounds, 
                                                        sumAgainstBlock, avgAgainstBlock, 
                                                        sumInFavoureOfBlock, avgInFavoureOfBlock,
                                                        gamesPlayed);
        }
    }
}
