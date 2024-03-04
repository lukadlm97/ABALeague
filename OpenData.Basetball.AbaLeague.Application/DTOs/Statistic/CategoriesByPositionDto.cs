using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Statistic
{
    public record CategoriesByPositionDto(int TeamId, 
                                            string TeamName, 
                                            int LeagueId, 
                                            string LeagueName,
                                            FrozenSet<CategoriesItemDto> Items);
}
