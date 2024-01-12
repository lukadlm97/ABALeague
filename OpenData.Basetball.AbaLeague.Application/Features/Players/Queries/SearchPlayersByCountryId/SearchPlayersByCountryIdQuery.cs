using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenQA.Selenium.PrintOptions;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Queries.SearchPlayersByCountryId
{
    public class SearchPlayersByCountryIdQuery : IQuery<Maybe<PlayersDto>>
    {
        public SearchPlayersByCountryIdQuery(int pageNumber, int pageSize, int countryId)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            CountryId = countryId;
        }
        public int PageNumber { get; private set; }
        public int PageSize { get; private set; }
        public int CountryId { get; private set; }
    }
}
