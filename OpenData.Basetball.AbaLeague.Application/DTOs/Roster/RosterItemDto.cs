

using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.DTOs.Player;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Roster
{
    public record RosterItemDto(
        int PlayerId,
        int LeagueId,
        DateTime Start,
        DateTime? End,
        string Name,
        PositionEnum Position,
        int Height,
        DateTime DateOfBirth,
        int Age,
        int CountryId,
        string CountryName) : PlayerItemDto(PlayerId,
                                            Name,
                                            Position, 
                                            Height, 
                                            DateOfBirth, 
                                            Age, 
                                            CountryId, 
                                            CountryName);

}
