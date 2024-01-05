﻿using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Commands.AddBoxscoreByRoundNo
{
    public class AddBoxscoreByRoundNoCommand : ICommand<Result>
    {
        public AddBoxscoreByRoundNoCommand(int leagueId, IEnumerable<AddBoxScoreDto> boxscores, int roundNo)
        {
            LeagueId = leagueId;
            Boxscores = boxscores;
            RoundNo = roundNo;
        }

        public int LeagueId { get; private set; }
        public IEnumerable<AddBoxScoreDto> Boxscores { get; private set; }
        public int RoundNo { get; private set; }
    }
}