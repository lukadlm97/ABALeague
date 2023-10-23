using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Contracts.Leagues
{
    public class LeaguesResponse
    {
        public IEnumerable<LeagueResponse> Leagues { get; set; }
    }
}
