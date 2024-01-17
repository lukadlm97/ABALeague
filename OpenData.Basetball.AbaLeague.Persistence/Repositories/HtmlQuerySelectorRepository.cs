using OpenData.Basetball.AbaLeague.Persistence;
using OpenData.Basketball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Persistence.Repositories
{
    public class HtmlQuerySelectorRepository : IHtmlQuerySelectorRepository
    {
        private readonly AbaLeagueDbContext _abaLeagueDbContext;

        public HtmlQuerySelectorRepository(AbaLeagueDbContext abaLeagueDbContext)
        {
            _abaLeagueDbContext = abaLeagueDbContext;
        }
        public IQueryable<HtmlQuerySelector> Get()
        {
            return _abaLeagueDbContext.HtmlQuerySelector;
        }
    }
}
