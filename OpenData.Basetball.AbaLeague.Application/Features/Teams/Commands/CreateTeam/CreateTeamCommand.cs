using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Commands.CreateTeam
{
    public class CreateTeamCommand : ICommand<Result>
    {
        public CreateTeamCommand(string name, string shortName, int? countryId)
        {
            Name = name;
            ShortName = shortName;
            CountryId = countryId;
        }
        public string Name { get; }
        public string ShortName { get; }
        public int? CountryId { get; }
    }
}
