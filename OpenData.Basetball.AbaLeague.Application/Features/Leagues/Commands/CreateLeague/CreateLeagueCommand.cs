using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Enums;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Commands.CreateLeague
{
    public sealed class CreateLeagueCommand : ICommand<Result>
    {
        public CreateLeagueCommand(string officialName,
            string shortName,
            string season,
            string standingUrl,
            string calendarUrl,
            string matchUrl,
            string boxScoreUrl,
            string baseUrl,
            string rosterUrl, 
            short processorType)
        {
            OfficialName = officialName;
            ShortName = shortName;
            Season = season;
            StandingUrl = standingUrl;
            CalendarUrl = calendarUrl;
            MatchUrl = matchUrl;
            BoxScoreUrl = boxScoreUrl;
            BaseUrl = baseUrl;
            RosterUrl = rosterUrl;
            ProcessorType = (Domain.Enums.ProcessorType) processorType;
        }

        public string OfficialName { get;  }
        public string ShortName { get;  }
        public string Season { get;  }
        public string StandingUrl { get;  }
        public string CalendarUrl { get;  }
        public string MatchUrl { get;  }
        public string BoxScoreUrl { get;  }
        public string BaseUrl { get;  }
        public string RosterUrl { get;  }
        public Domain.Enums.ProcessorType ProcessorType { get;  }
    }
}
