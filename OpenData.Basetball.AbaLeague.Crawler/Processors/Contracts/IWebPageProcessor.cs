﻿using OpenData.Basetball.AbaLeague.Domain.Entities;

namespace OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts
{
    public interface IWebPageProcessor
    {
        Task<IReadOnlyList<(string Name, string Url)>> GetTeams(string leagueUrl,
            CancellationToken cancellationToken=default);
        Task<IReadOnlyList<(int? No, string Name,string Position,decimal Height,DateTime DateOfBirth, string Nationality)>> 
            GetRoster(string teamUrl,
            CancellationToken cancellationToken = default);
        Task<IReadOnlyList<(int? Round,string HomeTeamName,string AwayTeamName,int HomeTeamPoints,int AwayTeamPoints,DateTime? Date,int? MatchNo)>> GetRegularSeasonCalendar(string calendarUrl,CancellationToken  cancellationToken=default);

        Task<IReadOnlyList<(int? Attendency, string Venue, int HomeTeamPoint, int AwayTeamPoint)>> GetMatchResult(
            IEnumerable<string> matchUrls, CancellationToken cancellationToken = default);
    }
}
