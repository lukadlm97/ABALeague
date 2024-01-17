using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using OpenData.Basketball.AbaLeague.Domain.Enums;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Commands.CreateLeague
{
    public sealed class CreateLeagueCommand : ICommand<Result>
    {
        public CreateLeagueCommand(string officialName,
            string shortName,
            string standingUrl,
            string calendarUrl,
            string matchUrl,
            string boxScoreUrl,
            string baseUrl,
            string rosterUrl, 
            short processorType,
            int seasonId,
            int roundsToPlay, 
            short competitionOrganization,
            string? standingsTable = null,
            string? standingsRowName = null,
            string? standingsRowUrl = null
            )
        {
            OfficialName = officialName;
            ShortName = shortName;
            StandingUrl = standingUrl;
            CalendarUrl = calendarUrl;
            MatchUrl = matchUrl;
            BoxScoreUrl = boxScoreUrl;
            BaseUrl = baseUrl;
            RosterUrl = rosterUrl;
            ProcessorType = (Domain.Enums.ProcessorType) processorType;
            SeasonId = seasonId;
            RoundsToPlay = roundsToPlay;
            CompetitionOrganization = (Domain.Enums.CompetitionOrganizationEnum) competitionOrganization;
            StandingsTableSelector = standingsTable;
            StandingsRowNameSelector = standingsRowName;
            StandingsRowUrlSelector = standingsRowUrl;
        }

        public string OfficialName { get;  }
        public string ShortName { get;  }
        public string StandingUrl { get;  }
        public string CalendarUrl { get;  }
        public string MatchUrl { get;  }
        public string BoxScoreUrl { get;  }
        public string BaseUrl { get;  }
        public string RosterUrl { get;  }
        public Domain.Enums.ProcessorType ProcessorType { get;  }
        public int SeasonId { get;  }
        public int RoundsToPlay { get;  }
        public Domain.Enums.CompetitionOrganizationEnum CompetitionOrganization { get;  }

        public string? StandingsTableSelector { get; }
        public string? StandingsRowNameSelector { get; }
        public string? StandingsRowUrlSelector { get; }
    }
}
