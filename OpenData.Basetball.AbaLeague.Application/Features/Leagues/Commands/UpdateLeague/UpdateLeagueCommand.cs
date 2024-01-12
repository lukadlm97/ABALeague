using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Leagues.Commands.UpdateLeague
{
    public class UpdateLeagueCommand : ICommand<Result>
    {
        public UpdateLeagueCommand(int id,
            string officialName,
            string shortName,
            string standingUrl,
            string calendarUrl,
            string matchUrl,
            string boxScoreUrl,
            string baseUrl,
            string rosterUrl,
            short processorType,
            int seasonId,
            int roundsToPlay)
        {
            Id = id;
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
        }

        public int Id { get; }
        public string OfficialName { get; }
        public string ShortName { get; }
        public string StandingUrl { get; }
        public string CalendarUrl { get; }
        public string MatchUrl { get; }
        public string BoxScoreUrl { get; }
        public string BaseUrl { get; }
        public string RosterUrl { get; }
        public Domain.Enums.ProcessorType ProcessorType { get; }
        public int SeasonId { get; }
        public int RoundsToPlay { get; }
    }
}
