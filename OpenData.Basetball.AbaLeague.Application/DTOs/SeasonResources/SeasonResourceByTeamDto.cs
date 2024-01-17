using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources
{
    public record SeasonResourceByTeamDto(int LeagueId,
                                            string LeagueName,
                                            string ShortName,
                                            string TeamName, 
                                            int TeamId, 
                                            string Url);
}
