using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Models;

namespace OpenData.Basetball.AbaLeague.Persistence.Repositories
{
    public class PlayerRepository:IPlayerRepository
    {
        private readonly AbaLeagueDbContext _abaLeagueDbContext;

        public PlayerRepository(AbaLeagueDbContext abaLeagueDbContext)
        {
            this._abaLeagueDbContext = abaLeagueDbContext;
        }
        public async Task<bool> Add(Player player)
        {
            try
            {
                await _abaLeagueDbContext.AddAsync(player);
                await _abaLeagueDbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }
    }
}
