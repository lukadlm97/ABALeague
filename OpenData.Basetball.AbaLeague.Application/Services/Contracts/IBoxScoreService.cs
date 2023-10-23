using OpenData.Basketball.AbaLeague.Application.DTOs.BoxScore;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface IBoxScoreService
    {
        Task<(IEnumerable<BoxScoreDto> homePlayers, IEnumerable<BoxScoreDto> awayPlayers, IEnumerable<string>
            missingPlayers)> GetScore(int leagueId, int matchNo, CancellationToken cancellationToken = default);
        Task<(IEnumerable<BoxScoreDto> playersScore, IEnumerable<string> missingPlayers)> GetRoundBoxScore(int leagueId, int roundNo, CancellationToken cancellationToken = default);
        Task<(IEnumerable<BoxScoreDto> playersScore, IEnumerable<string> missingPlayers)> GetEuroleagueRoundBoxScore(int leagueId, int roundNo, CancellationToken cancellationToken = default);
        Task<(IEnumerable<BoxScoreDto> playersScore, IEnumerable<string> missingPlayers)> GetEuroleagueMatchBoxScore(int leagueId, int matchNo, CancellationToken cancellationToken = default);
        Task<IEnumerable<(IEnumerable<BoxScoreDto> homePlayers, IEnumerable<BoxScoreDto> awayPlayers)>> AddScore(
            int leagueId, int matchId,IEnumerable<AddBoxScoreDto> homePlayers, IEnumerable<AddBoxScoreDto> awayPlayers, CancellationToken cancellationToken = default);
        Task<IEnumerable<BoxScoreDto>> AddScore(int leagueId,int roundNo, IEnumerable<AddBoxScoreDto> playerScores,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<BoxScoreDto>> AddScore(int leagueId, IEnumerable<AddBoxScoreDto> playerScores,
            CancellationToken cancellationToken = default);
    }
}
