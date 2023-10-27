using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.Utilities;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{
    public record RosterEntryDto(
        string Name,
        [property: JsonCamelCaseEnumConverter] PositionEnum Position,
        decimal Height, 
        DateTime DateOfBirth, 
        Basetball.AbaLeague.Domain.Entities.Country? Nationality,
        int? NationalityId);
}
