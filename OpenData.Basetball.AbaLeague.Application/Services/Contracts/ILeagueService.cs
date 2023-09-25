using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basketball.AbaLeague.Application.DTOs.League;
using OpenData.Basketball.AbaLeague.Application.DTOs.Round;

namespace OpenData.Basketball.AbaLeague.Application.Services.Contracts
{
    public interface ILeagueService
    {
        Task<IEnumerable<League>> Get(CancellationToken cancellationToken = default);
        Task<League> Get(int id, CancellationToken cancellationToken = default);
        Task Add(LeagueDto league, CancellationToken cancellationToken = default);
        Task Delete(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<RoundMatchDto>> GetCalendarDraft(int leagueId,CancellationToken cancellationToken = default);
        Task<IEnumerable<RoundMatchDto>> GetEuroleagueCalendarDraft(int leagueId,CancellationToken cancellationToken = default);
        Task<IEnumerable<RoundMatchDto>> GetEuroleagueCalendarSpecificRoundDraft(int leagueId, int roundNo, CancellationToken cancellationToken = default);
        Task<IEnumerable<RoundMatchDto>> GetExistingCalendar(int leagueId,CancellationToken cancellationToken = default);
        Task<IEnumerable<RoundMatchDto>> AddCalendar(int leagueId,IEnumerable<AddRoundMatchDto> entries,bool offSeason=false,CancellationToken cancellationToken = default);
    }
}
