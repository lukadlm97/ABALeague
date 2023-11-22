using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Players.Commands.UpdatePlayer
{
    public class UpdatePlayerCommand : ICommand<Result>
    {
        public UpdatePlayerCommand(int id,
            string name,
            int? positionId,
            int height,
            DateTime dateOfBirth,
            int? nationalityId)
        {
            Id = id;
            Name = name;
            Height = height;
            DateOfBirth = dateOfBirth;
            CountryId = nationalityId;
            PositionId = positionId;
        }
        public int Id { get; }
        public string Name { get; }
        public int Height { get; }
        public DateTime DateOfBirth { get; }
        public int? CountryId { get; }
        public int? PositionId { get; }
    }
}
