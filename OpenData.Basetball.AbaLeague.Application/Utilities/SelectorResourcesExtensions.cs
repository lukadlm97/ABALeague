using OpenData.Basketball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Utilities
{
    public static class SelectorResourcesExtensions
    {
        public static string? DetermianteSelectorValue(this List<ResourceSelector> selectors,
                                                        HtmlQuerySelectorEnum type)
        {
            var selector = selectors.FirstOrDefault(x => x.HtmlQuerySelectorEnum == Domain.Enums.HtmlQuerySelectorEnum.StandingsRowUrl);
            if(selector == null)
            {
                return string.Empty;
            }
            return selector.Value;
        }
    }
}
