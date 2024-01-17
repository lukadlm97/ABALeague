using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources
{
    public record AddSeasonResourceDto(int TeamId, 
                                        int LeagueId,
                                        string Url,
                                        string TeamName, 
                                        string? TeamUrl,
                                        string? IncrowdUrl,  
                                        string? Group = null,
                                        int? BracketPosition = null);
}
