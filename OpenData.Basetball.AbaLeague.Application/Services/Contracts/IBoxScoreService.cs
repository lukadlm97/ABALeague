using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface IBoxScoreService
    {
        Task<(IEnumerable<BoxScoreItemDto> homePlayers, IEnumerable<BoxScoreItemDto> awayPlayers, IEnumerable<string>
            missingPlayers)> GetScore(int leagueId, int matchNo, CancellationToken cancellationToken = default);
        Task<(IEnumerable<BoxScoreItemDto> playersScore, IEnumerable<string> missingPlayers)> GetRoundBoxScore(int leagueId, int roundNo, CancellationToken cancellationToken = default);
        Task<(IEnumerable<BoxScoreItemDto> playersScore, IEnumerable<string> missingPlayers)> GetEuroleagueRoundBoxScore(int leagueId, int roundNo, CancellationToken cancellationToken = default);
        Task<(IEnumerable<BoxScoreItemDto> playersScore, IEnumerable<string> missingPlayers)> GetEuroleagueMatchBoxScore(int leagueId, int matchNo, CancellationToken cancellationToken = default);
        Task<IEnumerable<(IEnumerable<BoxScoreItemDto> homePlayers, IEnumerable<BoxScoreItemDto> awayPlayers)>> AddScore(
            int leagueId, int matchId,IEnumerable<AddBoxScoreDto> homePlayers, IEnumerable<AddBoxScoreDto> awayPlayers, CancellationToken cancellationToken = default);
        Task<IEnumerable<BoxScoreItemDto>> AddScore(int leagueId,int roundNo, IEnumerable<AddBoxScoreDto> playerScores,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<BoxScoreItemDto>> AddScore(int leagueId, IEnumerable<AddBoxScoreDto> playerScores,
            CancellationToken cancellationToken = default);
    }
}
