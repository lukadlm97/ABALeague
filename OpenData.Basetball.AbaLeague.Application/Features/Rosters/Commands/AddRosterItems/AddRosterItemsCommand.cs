using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Commands.AddRosterItems
{
    public class AddRosterItemsCommand : ICommand<Result>
    {
        public int LeagueId { get; set; }
        public IEnumerable<AddRosterItemDto> RosterItems { get; set; }
        public AddRosterItemsCommand(int leagueId, IEnumerable<AddRosterItemDto> rosterItems)
        {
            LeagueId = leagueId;
            RosterItems = rosterItems;
        }
    }
}
