using OpenData.Basetball.AbaLeague.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basetball.AbaLeague.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AbaLeagueDbContext _context;
      //  private readonly IHttpContextAccessor _httpContextAccessor;
        private IPlayerRepository _playerRepository;


        public UnitOfWork(AbaLeagueDbContext context
            //,IHttpContextAccessor httpContextAccessor
            )
        {
            _context = context;
         //   this._httpContextAccessor = httpContextAccessor;
        }

        public IPlayerRepository PlayerRepository =>
            _playerRepository ??= new PlayerRepository(_context);

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
