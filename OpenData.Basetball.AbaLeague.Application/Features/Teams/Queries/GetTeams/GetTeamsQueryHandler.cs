using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeams
{
    internal class GetTeamsQueryHandler : IQueryHandler<GetTeamsQuery,Maybe<TeamResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTeamsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<TeamResponse>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
            {
                return Maybe<TeamResponse>.None;
            }
            var teams = await _unitOfWork.TeamRepository.GetAll(cancellationToken);

            if (teams == null)
            {
                return Maybe<TeamResponse>.None;
            }

            var teamResponse = new TeamResponse(teams.Skip((request.PageNumber-1)*request.PageSize).Take(request.PageSize).Select(x => new TeamDto(x.Id, x.Name, x.ShortCode, x.CountryId)));
            return teamResponse;

        }
    }
}
