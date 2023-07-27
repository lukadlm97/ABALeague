﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Contracts;
using OpenData.Basetball.AbaLeague.Crawler.Fetchers.Implementation;
using OpenData.Basetball.AbaLeague.Crawler.Models;
using OpenData.Basetball.AbaLeague.Crawler.Processors.Contracts;

namespace OpenData.Basetball.AbaLeague.Crawler.Processors.Implementations
{
    public class EuroPageProcessor : IWebPageProcessor
    {
        private readonly IDocumentFetcher _documentFetcher;

        public EuroPageProcessor(IDocumentFetcher documentFetcher)
        {
            _documentFetcher = documentFetcher;
        }
        public async Task<IReadOnlyList<(string Name, string Url)>> GetTeams(string leagueUrl, CancellationToken cancellationToken = default)
        {
            var webDocument = await _documentFetcher
                .FetchDocument(leagueUrl, cancellationToken);

            var teams = new List<(string, string)>();
            var teamElements = webDocument.QuerySelectorAll(".complex-stat-table_row__1P6us");

            bool initalCycle = true;
            foreach (var teamElement in teamElements)
            {
                if (initalCycle)
                {
                    initalCycle = false;
                    continue;
                }
                var name = teamElement
                    .QuerySelectorAll(".complex-stat-table_mainClubName__3IMZJ")[0]
                    .InnerHtml
                    .Trim();
                name = name.Substring(0,name.IndexOf('<'));

                var url = teamElement
                    .QuerySelectorAll(".complex-stat-table_clubWrap__2i3fk")[0]
                    .GetAttribute("href")
                    .Trim();


                teams.Add((name, url));
            }

            return teams;
        }

        public Task<IReadOnlyList<(int? No, string Name, string Position, decimal Height, DateTime DateOfBirth, string Nationality)>> GetRoster(string teamUrl, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<(int? Round, string HomeTeamName, string AwayTeamName, int HomeTeamPoints, int AwayTeamPoints, DateTime? Date, int? MatchNo)>> GetRegularSeasonCalendar(string calendarUrl, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<(int? Attendency, string Venue, int HomeTeamPoint, int AwayTeamPoint)>> GetMatchResult(IEnumerable<string> matchUrls, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<(IReadOnlyList<PlayerScore> HomeTeam, IReadOnlyList<PlayerScore> AwayTeam)> GetBoxScore(string matchUrl, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
