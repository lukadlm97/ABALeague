using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basketball.AbaLeague.Application.Contracts;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class CalendarRepository:ICalendarRepository
    {
        private readonly AbaLeagueDbContext _dbContext;

        public CalendarRepository(AbaLeagueDbContext dbContext)
        {
            _dbContext = dbContext;
        }
       
        public async Task<bool> Exist(int leagueId, int homeTeamId, int awayTeamId, CancellationToken cancellationToken = default)
        {
            var league = await  _dbContext.Leagues.FirstOrDefaultAsync(x=>x.Id== leagueId,cancellationToken);
            if (league == null)
            {
                return false;
            }

            return league.RoundMatches.Any(x => x.AwayTeamId == awayTeamId && x.HomeTeamId == homeTeamId);
        }
    }
}
