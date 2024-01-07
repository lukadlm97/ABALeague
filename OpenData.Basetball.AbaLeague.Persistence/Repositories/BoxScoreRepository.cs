﻿using System;
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
    public class BoxScoreRepository : GenericRepository<BoxScore>, IBoxScoreRepository
    {
        public BoxScoreRepository(AbaLeagueDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> Exist(int roundMatchId, int rosterItemId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.BoxScores.AnyAsync(
                x => x.RoundMatchId == roundMatchId && x.RosterItemId == rosterItemId, cancellationToken);
        }

        public async Task<IEnumerable<BoxScore>>
            GetByRosterItemId(int rosterItemId, CancellationToken cancellationToken = default)
        {
            return _dbContext.BoxScores
                                .Include(x=>x.RoundMatch)
                                    .ThenInclude(x=>x.AwayTeam)
                                .Include(x=>x.RoundMatch)
                                    .ThenInclude(x => x.HomeTeam)
                                .Where(x => x.RosterItemId == rosterItemId);
        }

        public async Task<IEnumerable<BoxScore>> SearchByLeagueId(int leagueId, CancellationToken cancellationToken = default)
        {
            var league = await _dbContext.Leagues
                                        .Include(x=>x.RoundMatches)
                                        .FirstOrDefaultAsync(x=>x.Id == leagueId);
            if(league == null)
            {
                return Array.Empty<BoxScore>();
            }
            var existingRoundMatches = league.RoundMatches.Select(x => x.Id);
            return _dbContext.BoxScores
                                .Where(x => existingRoundMatches.Contains(x.RoundMatchId));
        }
    }
}
