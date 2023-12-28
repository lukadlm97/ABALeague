using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.CreatePlayers
{
    public class CreatePlayersCommand : ICommand<Result>
    {
        public CreatePlayersCommand(IEnumerable<AddPlayerDto> playerItems)
        {
            PlayerItems = playerItems;
        }

        public IEnumerable<AddPlayerDto> PlayerItems { get; private set; }
    }
}
