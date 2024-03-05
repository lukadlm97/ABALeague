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

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class RosterRepository : GenericRepository<RosterItem>, IRosterRepository
    {
        public RosterRepository(AbaLeagueDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> Exist(int leagueId, int playerId, int teamId, CancellationToken cancellationToken = default)
        {
            var team = _dbContext.Teams.Include(x=>x.RosterItems).FirstOrDefault(x => x.Id == teamId);
            if (team == null)
            {
                return false;
            }

            var roster = team.RosterItems;

            return roster.Any(x=> x.PlayerId == playerId && x.LeagueId == leagueId);
        }

        public async Task<RosterItem?> Get(int leagueId, int playerId, int teamId, CancellationToken cancellationToken = default)
        {
            var team = _dbContext.Teams.Include(x => x.RosterItems).FirstOrDefault(x => x.Id == teamId);
            if (team == null)
            {
                return null;
            }

            var roster = team.RosterItems;

            return roster.FirstOrDefault(x => x.PlayerId == playerId && x.LeagueId == leagueId);
        }

        public async Task<RosterItem?> Get(int leagueId, int playerId, CancellationToken cancellationToken = default)
        {
            return _dbContext.RosterItems
                                .Include(x => x.Player)
                                .Include(x => x.League)
                                .FirstOrDefault(x => x.PlayerId == playerId && x.LeagueId == leagueId);
        }

        public async Task<IEnumerable<RosterItem>> SearchByLeagueId(int leagueId, 
            CancellationToken cancellationToken = default)
        {
            var league = _dbContext.Leagues.FirstOrDefaultAsync(x=>x.Id == leagueId, cancellationToken);
            if (league == null)
            {
                return Array.Empty<RosterItem>();
            }

            return _dbContext.RosterItems
                .Include(x=>x.Team)
                .Include(x=>x.Player)
                .Include(x=>x.League)
                .Where(x => x.LeagueId == leagueId);
        }

        public async Task<IEnumerable<RosterItem>> 
            GetTeamRosterByLeagueId(int leagueId, int teamId, CancellationToken cancellationToken = default)
        {
            var team = await _dbContext.Teams.Include(x => x.RosterItems)
                                        .FirstOrDefaultAsync(x => x.Id == teamId);
            if (team == null)
            {
                return Array.Empty<RosterItem>();
            }

            return team.RosterItems.ToList()
                        .Where(x => x.LeagueId == leagueId);
        }

        public async Task<IQueryable<RosterItem>> 
            SearchRosterByLeagueAndTeamId(int leagueId, int teamId, CancellationToken cancellationToken = default)
        {
            var league = await _dbContext.Leagues.FirstOrDefaultAsync(x => x.Id == leagueId, cancellationToken);
            if (league == null)
            {
                return Array.Empty<RosterItem>().AsQueryable();
            }

            return _dbContext.RosterItems
                .Include(x => x.Team)
                .Include(x => x.Player)
                .Include(x => x.League)
                .Where(x => x.LeagueId == leagueId && x.TeamId == teamId);
        }

        public IQueryable<RosterItem> Get()
        {
            return _dbContext.RosterItems;
        }
    }
}
