using OpenData.Basketball.AbaLeague.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basetball.AbaLeague.Persistence.Repositories;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System.Threading;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class ResultRepository : GenericRepository<Result>, IResultRepository
    {
        public ResultRepository(AbaLeagueDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> Exist(int matchRoundId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Results.AnyAsync(x => x.RoundMatchId == matchRoundId, cancellationToken);
        }

       
        public async Task<Result?> GetByMatchRound(int matchRoundId, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Results
                                    .FirstOrDefaultAsync(x => x.RoundMatchId == matchRoundId, cancellationToken);
        }

        public async Task<IEnumerable<Result>> SearchByLeague(int leagueId, CancellationToken cancellationToken = default)
        {
            var league = await _dbContext.Leagues
                .Include(x=>x.RoundMatches)
                .FirstOrDefaultAsync(x => x.Id == leagueId, cancellationToken);
            if (league == null)
            {
                return Array.Empty<Result>();
            }

            var results = _dbContext.Results
                              .Include(x => x.RoundMatch);

            List<Result> finalResultSet = new List<Result>();
            foreach(var roundMatch in league.RoundMatches)
            {
                if(results.Any(x=>x.RoundMatchId==roundMatch.Id))
                {
                    var result = results.FirstOrDefault(x => x.RoundMatchId == roundMatch.Id);
                    finalResultSet.Add(result);
                }
            }

            return finalResultSet;
        }

        public  IQueryable<Result> SearchByLeagueAndRoundNo(int leagueId, int roundNo, CancellationToken cancellationToken = default)
        {
            var league =  _dbContext.Leagues
                .Include(x => x.RoundMatches)
                .FirstOrDefault(x => x.Id == leagueId);
            if (league == null)
            {
                return Array.Empty<Result>().AsQueryable();
            }

            var results = _dbContext.Results;

            List<Result> finalResultSet = new List<Result>();
            foreach (var roundMatch in league.RoundMatches)
            {
                if (results.Any(x => x.RoundMatchId == roundMatch.Id && x.RoundMatch.Round == roundNo))
                {
                    var result = results
                        .FirstOrDefault(x => x.RoundMatchId == roundMatch.Id && 
                    x.RoundMatch.Round == roundNo);

                    if(result == null)
                    {
                        continue;
                    }
                    finalResultSet.Add(result);
                }
            }

            return finalResultSet.AsQueryable();
        }

        public async Task<Result?> GetByMatchResult(int matchResultId, CancellationToken cancellationToken = default)
        {
           return await _dbContext.Results
                                  .Include(x => x.RoundMatch)
                                      .ThenInclude(x=> x.HomeTeam)
                                  .Include(x => x.RoundMatch)
                                      .ThenInclude(x=> x.AwayTeam)
                                  .FirstOrDefaultAsync(x=> x.Id == matchResultId, cancellationToken);    
        }

        public IQueryable<Result> Get()
        {
            return _dbContext.Results;
        }
    }
}
