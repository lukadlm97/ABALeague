using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class CalendarRepository:GenericRepository<RoundMatch>,ICalendarRepository
    {
        private readonly AbaLeagueDbContext _dbContext;

        public CalendarRepository(AbaLeagueDbContext dbContext):base(dbContext)
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

        public async Task<IEnumerable<RoundMatch>> SearchByRound(int leagueId, int round, CancellationToken cancellationToken = default)
        {
            var league = await _dbContext.Leagues
                .Include(x=>x.RoundMatches)
                    .ThenInclude(x=>x.AwayTeam)
                .Include(x=>x.RoundMatches)
                    .ThenInclude(x=>x.HomeTeam)
                .FirstOrDefaultAsync(x => x.Id== leagueId, cancellationToken);
            if (league == null)
            {
                return Array.Empty<RoundMatch>();
            }

            var filtered = league.RoundMatches.Where(x=>x.Round==round);
            return filtered;
        }

        public async Task<IEnumerable<RoundMatch>> SearchByLeague(int leagueId, CancellationToken cancellationToken = default)
        {
            var league = await _dbContext.Leagues
                .Include(x => x.RoundMatches)
                .ThenInclude(x => x.AwayTeam)
                .Include(x => x.RoundMatches)
                .ThenInclude(x => x.HomeTeam)
                .FirstOrDefaultAsync(x => x.Id == leagueId, cancellationToken);
            if (league == null)
            {
                return Array.Empty<RoundMatch>();
            }
            return league.RoundMatches;
        }

        public async Task<RoundMatch?> SearchByMatchNo(int leagueId,int matchNo, CancellationToken cancellationToken = default)
        {
            var league = await _dbContext.Leagues.Include(x=>x.RoundMatches).FirstOrDefaultAsync(x => x.Id == leagueId,cancellationToken);

            return league.RoundMatches.FirstOrDefault(x => x.MatchNo == matchNo);
        }

        public async Task<IEnumerable<RoundMatch>> SearchByRoundNo(int leagueId, int roundNo, CancellationToken cancellationToken = default)
        {
            var league = await _dbContext.Leagues.Include(x => x.RoundMatches).FirstOrDefaultAsync(x => x.Id == leagueId, cancellationToken);
            return league.RoundMatches.Where(x => x.Round == roundNo);
        }
    }
}
