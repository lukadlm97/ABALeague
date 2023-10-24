using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.SeasonResources;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.SeasonResources.Commands.CreateSeasonResources
{
    public class CreateSeasonResourcesCommand : ICommand<Result>
    {
        public CreateSeasonResourcesCommand(IEnumerable<AddSeasonResourceDto> seasonResources)
        {
            SeasonResource = seasonResources;
        }

        public IEnumerable<AddSeasonResourceDto> SeasonResource { get; set; }
    }
}
