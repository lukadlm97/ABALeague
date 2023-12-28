using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Commands.RemoveRosterItems
{
    public class RemoveRosterItemsCommand : ICommand<Result>
    {
        public RemoveRosterItemsCommand(IEnumerable<RemoveRosterItemDto> removeRosterItems) 
        {
            RemoveRosterItems = removeRosterItems;
        }

        public IEnumerable<RemoveRosterItemDto> RemoveRosterItems { get; private set; }
    }
}
