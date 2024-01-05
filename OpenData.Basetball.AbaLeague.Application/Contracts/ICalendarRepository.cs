using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface ICalendarRepository:IGenericRepository<RoundMatch>
    {
        Task<bool> Exist(int leagueId,int homeTeamId,int awayTeamId,CancellationToken cancellationToken=default);
        Task<IEnumerable<RoundMatch>> SearchByRound(int leagueId, int round,CancellationToken cancellationToken=default);
        Task<IEnumerable<RoundMatch>> SearchByLeague(int leagueId,CancellationToken cancellationToken=default);
        Task<RoundMatch?> SearchByMatchNo(int leagueId,int matchNo,CancellationToken cancellationToken=default);
        Task<IEnumerable<RoundMatch>> SearchByRoundNo(int leagueId, 
                                                        int roundNo, 
                                                        CancellationToken cancellationToken=default);
        Task<IEnumerable<RoundMatch>> SearchByLeagueIdAndTeamId(int leagueId,
                                                                    int teamId,
                                                                    CancellationToken cancellationToken = default);
    }
}
