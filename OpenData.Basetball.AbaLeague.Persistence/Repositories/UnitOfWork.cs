using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.Contracts;
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
