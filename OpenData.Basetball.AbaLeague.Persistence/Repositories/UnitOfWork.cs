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
      //  private readonly IHttpContextAccessor _httpContextAccessor;
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
        private IGenericRepository<ProcessorType> _processorTypeRepository;


        public UnitOfWork(AbaLeagueDbContext context
            //,IHttpContextAccessor httpContextAccessor
            )
        {
            _context = context;
         //   this._httpContextAccessor = httpContextAccessor;
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

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
          //  var username = _httpContextAccessor.HttpContext.User.FindFirst(CustomClaimTypes.Uid)?.Value;

            await _context.SaveChangesAsync();
        }
    }
}
