using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.DTOs.Player
{
    public record PlayerItemDto(string Name,
                                [property: JsonCamelCaseEnumConverter] PositionEnum Position,
                                int Height,
                                DateTime DateOfBirth,
                                int? NationalityId);
}
