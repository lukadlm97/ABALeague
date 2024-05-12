using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class LeagueRepository : GenericRepository<League>, ILeagueRepository
    {
        public LeagueRepository(AbaLeagueDbContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<League> Get()
        {
            return _dbContext.Leagues;
        }

        public IQueryable<League> GetIncludedRoundMatches()
        {
            return _dbContext.Leagues
                            .Include(x => x.RoundMatches)
                                .ThenInclude(y => y.HomeTeam)
                            .Include(x => x.RoundMatches)
                                .ThenInclude(x => x.AwayTeam);
        }

        public League? SearchLeagueByRoundMatchId(int roundMatchId)
        {
            var roundMatch = _dbContext.RoundMatches.FirstOrDefault(x => x.Id == roundMatchId);
            if(roundMatch == null)
            {
                return null;
            }
            Dictionary<League, IEnumerable<int>> leaguesWithRoundMatchIds = new Dictionary<League, IEnumerable<int>>();

            foreach(var league in _dbContext.Leagues)
            {
                if(league.RoundMatches == null)
                {
                    leaguesWithRoundMatchIds.Add(league,new List<int>());
                    continue;
                }
                leaguesWithRoundMatchIds.Add(league, league.RoundMatches.Select(x => x.Id));
            }

            foreach(var (league, ids) in leaguesWithRoundMatchIds)
            {
                if (ids.Contains(roundMatchId))
                {
                    return league;
                }
            }

            return null;
        }
    }
}
