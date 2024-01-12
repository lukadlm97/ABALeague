using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources
{
    public record TeamSeasonResourceDto(FrozenSet<TeamItemDraftDto> ExistingTeamSeasonResourcesItems,
                                        FrozenSet<TeamItemDraftDto> DraftTeamSeasonResourcesItems,
                                        FrozenSet<string> MissingTeamItems);
}
