using OpenData.Basketball.AbaLeague.Domain.Enums;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Team
{
    public record TeamDto(string? Name,string? ShortName, string? Iso3Code);
    public record TeamDTO(int? TeamId, string? Name, string? Url, string? TeamUrl, string? IncrowdUrl, 
        MaterializationStatus MaterializationStatus = MaterializationStatus.NotExist);
}
