﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basketball.AbaLeague.Application.Contracts
{
    public interface IRosterRepository : IGenericRepository<RosterItem>
    {
        Task<bool> Exist(int leagueId, int playerId, int teamId, CancellationToken  cancellationToken = default);
        Task<RosterItem?> Get(int leagueId, int playerId, int teamId, CancellationToken  cancellationToken = default);
    }
}
