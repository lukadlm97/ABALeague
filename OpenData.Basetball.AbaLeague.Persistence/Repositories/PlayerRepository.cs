
using Microsoft.EntityFrameworkCore;
using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Persistence.Repositories
{
    public class PlayerRepository: GenericRepository<Player>, IPlayerRepository
    {
        public PlayerRepository(AbaLeagueDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<bool> AddAnotherName(int playerId, string name, CancellationToken cancellationToken = default)
        {
            var player = await _dbContext.Players
                .Include(x=>x.AnotherNameItems)
                .FirstOrDefaultAsync(x=> x.Id == playerId, cancellationToken);
            if(player == null)
            {
                return false;
            }

            if(player.AnotherNameItems==null || !player.AnotherNameItems.Any() )
            {
                player.AnotherNameItems = new List<AnotherNameItem>();
            }

            player.AnotherNameItems.Add(new AnotherNameItem { Name = name });
            return true;
        }

        public async Task<bool> Exist(string name, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Players.AnyAsync(x => x.Name == name, cancellationToken);
        }

        public async Task<bool> ExistAnotherName(int playerId, string name, CancellationToken cancellationToken = default)
        {
            var player = await _dbContext.Players
                .Include(x => x.AnotherNameItems)
                .FirstOrDefaultAsync(x => x.Id == playerId, cancellationToken);
            if (player == null)
            {
                return false;
            }

            if (!player.AnotherNameItems.Any())
            {
                return false;
            }

            return player.AnotherNameItems.Any(x=> x.Name.ToLower() == name.ToLower());
        }

        public async Task<Player> Get(string name, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Players.SingleOrDefaultAsync(x => x.Name == name, cancellationToken);
        }

        public async Task<AnotherNameItem?> GetPlayerByAnotherName(string name, CancellationToken cancellationToken = default)
        {
            var players = _dbContext.Players.Include(x => x.AnotherNameItems);
            var anotherNames = players.SelectMany(x => x.AnotherNameItems);

            return await anotherNames.FirstOrDefaultAsync(x=> x.Name.ToLower() == name.ToLower(), cancellationToken);
        }

        public async Task<IEnumerable<AnotherNameItem>> GetAnotherNames(int playerId, CancellationToken cancellationToken = default)
        {
            var player= await _dbContext.Players
                                            .Include(x=>x.AnotherNameItems)
                                            .FirstOrDefaultAsync(x=>x.Id == playerId, cancellationToken);
            if (player == null)
            {
                return new List<AnotherNameItem>(); 
            }
            
            return player.AnotherNameItems;
        }
    }
}
