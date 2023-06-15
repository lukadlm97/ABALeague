using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.Utilities;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Player
{
    public record AddPlayerDto(
        string Name,
        [property: JsonCamelCaseEnumConverter] PositionEnum Position,
        int Height,
        DateTime DateOfBirth,
        int? NationalityId);
}
