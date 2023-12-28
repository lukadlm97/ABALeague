﻿using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreDraftByMatchResultId
{
    public class GetBoxscoreDraftByMatchResultIdQuery : IQuery<Maybe<BoxScoreDto>>
    {
        public GetBoxscoreDraftByMatchResultIdQuery(int leagueId, int matchResultId)
        {
            MatchResultId = matchResultId;
            LeagueId = leagueId;
        }

        public int MatchResultId { get; private set; }
        public int LeagueId { get; private set; }
    }
}
