using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.SearchPlayers
{
    public class SearchPlayersQuery : IQuery<Maybe<PlayersDto>>
    {
        public SearchPlayersQuery(int pageNumber, int pageSize, string filter)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Filter = filter;
        }

        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public string Filter { get; private set; }
    }
}
