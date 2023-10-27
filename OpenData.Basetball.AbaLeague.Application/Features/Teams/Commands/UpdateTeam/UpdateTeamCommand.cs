using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Commands.UpdateTeam
{
    public class UpdateTeamCommand : ICommand<Result>
    {
        public UpdateTeamCommand(int id, string name, string shortName, int? countryId)
        {
            Id = id;
            Name = name;
            ShortName = shortName;
            CountryId = countryId;
        }
        public int Id { get; }
        public string Name { get; }
        public string ShortName { get; }
        public int? CountryId { get; }
    }
}
