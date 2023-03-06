using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Domain.Models;

namespace OpenData.Basetball.AbaLeague.Persistence.Repositories
{
    public interface IPlayerRepository
    {
        Task<bool> Add(Player player);
    }
}
