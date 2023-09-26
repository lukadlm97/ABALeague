
using OpenData.Basketball.AbaLeague.Application.DTOs;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface IResultService
    {
        Task<IEnumerable<ResultDto>> GetResultsByRoundId(int leagueId,int roundId,CancellationToken cancellationToken=default);
        Task<IEnumerable<ResultDto>> GetResults(int leagueId, CancellationToken cancellationToken=default);
        Task<IEnumerable<ResultDto>> GetEuroleagueResults(int leagueId, CancellationToken cancellationToken=default);

        Task<IEnumerable<ResultDto>> GetEuroleagueResults(int leagueId, int matchNo,
            CancellationToken cancellationToken = default);
        Task<IEnumerable<ResultDto>> Add(int leagueId, IEnumerable<AddResultDto> entries, CancellationToken cancellationToken=default);
    }
}
