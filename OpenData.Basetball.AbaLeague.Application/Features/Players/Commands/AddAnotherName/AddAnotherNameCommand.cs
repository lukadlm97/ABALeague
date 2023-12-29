using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.AddAnotherName
{
    public class AddAnotherNameCommand : ICommand<Result>
    {
        public AddAnotherNameCommand(int playerId, string name)
        {
            PlayerId = playerId;
            Name = name;
        }

        public int PlayerId { get; private set; }
        public string Name { get; private set; }
    }
}
