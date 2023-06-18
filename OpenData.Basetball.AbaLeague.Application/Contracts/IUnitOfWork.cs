﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Contracts;

namespace OpenData.Basetball.AbaLeague.Application.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IPlayerRepository PlayerRepository { get; }
        ITeamRepository TeamRepository { get; }
        IGenericRepository<League> LeagueRepository { get; }
        ISeasonResourcesRepository SeasonResourcesRepository { get; }
        ICountryRepository CountryRepository { get; }
        IPositionRepository PositionRepository { get; }
        ICalendarRepository CalendarRepository { get; }
        IResultRepository ResultRepository { get; }
        IBoxScoreRepository BoxScoreRepository { get; }
        IRosterRepository RosterRepository { get; }
        Task Save();
    }
}
