using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Persistence.Repositories;

namespace OpenData.Basetball.AbaLeague.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AbaLeagueDbContext _context;
        private IPlayerRepository _playerRepository;
        private ITeamRepository _teamRepository;
        private IGenericRepository<League> _leagueRepository;
        private ISeasonResourcesRepository _seasonResourcesRepository;
        private ICountryRepository _countryRepository;
        private IPositionRepository _positionRepository;
        private ICalendarRepository _calendarRepository;
        private IResultRepository _resultRepository;
        private IBoxScoreRepository _boxScoreRepository;
        private IRosterRepository _rosterRepository;
        private IResourceSelectorRepository _selectorResourcesRepository;
        private IGameLengthRepository _gameLengthRepository;
        private ILeagueGameLengthRepository _leagueGameLengthRepository;
        private ILevelsOfScaleRepository _levelsOfScaleRepository;
        private IRangeScalesRepository _rangeScalesRepository;
        private IStatsPropertyRepository _statsPropertyRepository;
        private IGenericRepository<ProcessorType> _processorTypeRepository;
        private ISeasonRepository _seasonRepository;
        private ICompetitionOrganizationRepository _competitionOrganizationRepository;
        private IHtmlQuerySelectorRepository _htmlQuerySelectorRepository;

        public UnitOfWork(AbaLeagueDbContext context)
        {
            _context = context;
        }

        public IPlayerRepository PlayerRepository =>
            _playerRepository ??= new PlayerRepository(_context);
        

        public IGenericRepository<League> LeagueRepository =>
            _leagueRepository ??= new GenericRepository<League>(_context);

        public ITeamRepository TeamRepository =>
            _teamRepository ??= new TeamRepository(_context);

        public ISeasonResourcesRepository SeasonResourcesRepository =>
            _seasonResourcesRepository ??= new SeasonResourcesRepository(_context);

        public ICountryRepository CountryRepository =>
            _countryRepository ??= new CountryRepository(_context);

        public IPositionRepository PositionRepository =>
            _positionRepository ??= new PositionRepository(_context);

        public ICalendarRepository CalendarRepository =>
            _calendarRepository ??= new CalendarRepository(_context);
        public IResultRepository ResultRepository =>
            _resultRepository ??= new ResultRepository(_context);
        public IBoxScoreRepository BoxScoreRepository =>
            _boxScoreRepository ??= new BoxScoreRepository(_context);

        public IRosterRepository RosterRepository =>
            _rosterRepository ??= new RosterRepository(_context);

        public IGenericRepository<ProcessorType> ProcessorTypeRepository =>
          _processorTypeRepository ??= new GenericRepository<ProcessorType>(_context);

        public IResourceSelectorRepository SelectorResourcesRepository =>
          _selectorResourcesRepository ??= new ResourceSelectorRepository(_context);

        public IGameLengthRepository GameLengthRepository =>
          _gameLengthRepository ??= new GameLengthRepository(_context);

        public ILeagueGameLengthRepository LeagueGameLengthRepository =>
          _leagueGameLengthRepository ??= new LeagueGameLengthRepository(_context);

        public ILevelsOfScaleRepository LevelsOfScaleRepository =>
          _levelsOfScaleRepository ??= new LevelsOfScaleRepository(_context);

        public IRangeScalesRepository RangeScalesRepository =>
          _rangeScalesRepository ??= new RangeScalesRepository(_context);

        public IStatsPropertyRepository StatsPropertyRepository =>
          _statsPropertyRepository ??= new StatsPropertyRepository(_context);

        public ISeasonRepository SeasonRepository =>
          _seasonRepository ??= new SeasonRepository(_context);
        
        public ICompetitionOrganizationRepository CompetitionOrganizationRepository =>
          _competitionOrganizationRepository ??= new CompetitionOrganizationRepository(_context);

        public IHtmlQuerySelectorRepository HtmlQuerySelectorRepository =>
          _htmlQuerySelectorRepository ??= new HtmlQuerySelectorRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken: cancellationToken);
        }
    }
}
