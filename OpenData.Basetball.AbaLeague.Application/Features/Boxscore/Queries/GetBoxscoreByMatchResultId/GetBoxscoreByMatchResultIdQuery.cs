using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Boxscore.Queries.GetBoxscoreByMatchResultId
{
    public class GetBoxscoreByMatchResultIdQuery : IQuery<Maybe<BoxscoreByMatchResultDto>>
    {
        public GetBoxscoreByMatchResultIdQuery(int matchResultId)
        {
            MatchResultId = matchResultId;
        }

        public int MatchResultId { get; init; }
    }
}
