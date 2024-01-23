using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.League
{
    public record StandingsDto(int LeagueId,
                                string LeagueName,
                                Domain.Enums.CompetitionOrganizationEnum? LeagueCompetitionOrganization,
                                int TotalRounds,
                                int PlayedRounds,
                                FrozenSet<StandingsItemDto>? StandingItems = null,
                                FrozenSet<GroupStandingsDto>? GroupStandingItems = null,
                                FrozenSet<BracketStandingsDto>? BracketStandingItems = null
                                );
}
