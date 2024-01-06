using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface IStandingsService
    {
        Task<IEnumerable<StandingsItemDto>> 
            GetByLeagueId(int leagueId,  
                                            CancellationToken cancellationToken = default);
    }
}
