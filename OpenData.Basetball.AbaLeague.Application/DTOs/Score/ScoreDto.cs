using OpenData.Basketball.AbaLeague.Application.DTOs.Schedule;
using OpenData.Basketball.AbaLeague.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Score
{
    public record ScoreDto(IEnumerable<ScoreItemDto> DraftScoreItems,
                                IEnumerable<ScoreItemDto> ExistingScoreItems, 
                                IEnumerable<ScoreItemDto> PlannedToPlayItems);
}