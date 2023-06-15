using OpenData.Basketball.AbaLeague.Application.DTOs.Player;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface IPlayerService
    {
        Task<IEnumerable<PlayerDto>> Get(CancellationToken cancellationToken = default);
        Task<IEnumerable<PlayerDto>> Get(int teamId,CancellationToken cancellationToken = default);
        Task<IEnumerable<PlayerDto>> Add(AddPlayerDto addPlayerDto,CancellationToken cancellationToken = default);
        Task<IEnumerable<PlayerDto>> Add(IEnumerable<AddPlayerDto> addPlayerDtoList,CancellationToken cancellationToken = default);
    }
}
