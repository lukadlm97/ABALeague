﻿using OpenData.Basetball.AbaLeague.Domain.Enums;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.CreatePlayer
{
    public class CreatePlayerCommand : ICommand<Result>
    {
        public CreatePlayerCommand(string name,
            int? positionId,
            int height,
            DateTime dateOfBirth,
            int? nationalityId)
        {
            Name = name;
            Height = height;
            DateOfBirth = dateOfBirth;
            CountryId = nationalityId;
            PositionId = positionId;
        }
        public string Name { get;  }
        public int Height { get; }
        public DateTime DateOfBirth { get; }
        public int? CountryId { get; }
        public int? PositionId { get; }
    }
}
