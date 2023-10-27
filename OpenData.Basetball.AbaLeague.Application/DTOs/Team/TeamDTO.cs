using OpenData.Basketball.AbaLeague.Domain.Enums;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Team
{
    public record TeamResponse(IEnumerable<TeamDto> Teams);
    public record TeamDto(int? Id, string? Name,string? ShortName, int? CountryId);
    public record TeamDTO(int? TeamId, string? Name, string? Url, string? TeamUrl, string? IncrowdUrl, 
        MaterializationStatus MaterializationStatus = MaterializationStatus.NotExist);

}
