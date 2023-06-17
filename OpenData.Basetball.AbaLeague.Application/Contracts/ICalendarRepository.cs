using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface ICalendarRepository
    {
        Task<bool> Exist(int leagueId,int homeTeamId,int awayTeamId,CancellationToken cancellationToken=default);
    }
}
