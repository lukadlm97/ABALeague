using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayerAnotherNames
{
    public class GetPlayerAnotherNamesQuery : IQuery<Maybe<AnotherNameDto>>
    {
        public GetPlayerAnotherNamesQuery(int playerId)
        {
            PlayerId = playerId;
        }

        public int PlayerId { get; private set; }
    }
}
