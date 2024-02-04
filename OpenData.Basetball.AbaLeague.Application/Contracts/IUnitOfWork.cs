using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Application.Contracts
{
    public interface IUnitOfWork : IDisposable
    {
        IPlayerRepository PlayerRepository { get; }
        ITeamRepository TeamRepository { get; }
        ILeagueRepository LeagueRepository { get; }
        IGenericRepository<ProcessorType> ProcessorTypeRepository { get; }
        ISeasonResourcesRepository SeasonResourcesRepository { get; }
        ICountryRepository CountryRepository { get; }
        IPositionRepository PositionRepository { get; }
        ICalendarRepository CalendarRepository { get; }
        IResultRepository ResultRepository { get; }
        IBoxScoreRepository BoxScoreRepository { get; }
        IRosterRepository RosterRepository { get; }
        IResourceSelectorRepository SelectorResourcesRepository { get; }
        IGameLengthRepository GameLengthRepository { get; }
        ILeagueGameLengthRepository LeagueGameLengthRepository { get; }
        ILevelsOfScaleRepository LevelsOfScaleRepository { get; }
        IRangeScalesRepository RangeScalesRepository { get; }
        IStatsPropertyRepository StatsPropertyRepository { get; }
        ISeasonRepository SeasonRepository { get; }
        ICompetitionOrganizationRepository CompetitionOrganizationRepository { get; }
        IHtmlQuerySelectorRepository HtmlQuerySelectorRepository { get; }
        Task Save(CancellationToken cancellationToken = default);
    }
}
