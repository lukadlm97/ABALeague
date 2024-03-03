using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.GetPlayersByCountryId
{
    public class GetPlayersByCountryIdQuery : IQuery<Maybe<PlayerItemsByCountryDto>>
    {
        public GetPlayersByCountryIdQuery(int countryId)
        {
            CountryId = countryId;
        }

        public int CountryId { get; }
    }
}
