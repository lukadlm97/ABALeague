using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.SeasonResources.Commands.CreateSeasonResources
{
    internal class CreateSeasonResourcesHandler : ICommandHandler<CreateSeasonResourcesCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSeasonResourcesHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CreateSeasonResourcesCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<Basetball.AbaLeague.Domain.Entities.SeasonResources> list =
                    new List<Basetball.AbaLeague.Domain.Entities.SeasonResources>();
                foreach (var addSeasonResourceDto in request.SeasonResource)
                {
                    var teamResources =
                        await _unitOfWork.SeasonResourcesRepository.SearchByTeam(addSeasonResourceDto.TeamId,
                            cancellationToken);
                    if (teamResources.Any(x => x.LeagueId == addSeasonResourceDto.LeagueId))
                    {
                        continue;
                    }
                    list.Add(new Basetball.AbaLeague.Domain.Entities.SeasonResources()
                    {
                        TeamSourceUrl = addSeasonResourceDto.Url,
                        LeagueId = addSeasonResourceDto.LeagueId,
                        TeamId = addSeasonResourceDto.TeamId,
                        TeamName = addSeasonResourceDto.TeamName,
                        TeamUrl = addSeasonResourceDto.TeamUrl??string.Empty,
                        IncrowdUrl = addSeasonResourceDto.IncrowdUrl
                    });
                }

                await _unitOfWork.SeasonResourcesRepository.Add(list, cancellationToken);
                await _unitOfWork.Save();

                return Result.Success();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Result.Failure(new Error("",""));
            }
        }
    }
}
