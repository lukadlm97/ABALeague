using OpenData.Basetball.AbaLeague.Domain.Entities;
using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.Utilities;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Player
{
    public record PlayerDto(
        int Id,
        string Name,
        [property: JsonCamelCaseEnumConverter] PositionEnum Position,
        decimal Height,
        DateTime DateOfBirth,
        Basetball.AbaLeague.Domain.Entities.Country Nationality);

    public record PlayerDTO(int Id, string Name, PositionEnum Position, int Height,
        DateTime DateOfBirth, int CountryId);

    public record PlayerResponse(IEnumerable<PlayerDTO> Players);
}
