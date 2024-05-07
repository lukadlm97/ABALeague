using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System.Collections.Frozen;
using System.Linq;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeams
{
    internal class GetTeamsQueryHandler : IQueryHandler<GetTeamsQuery,Maybe<TeamDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTeamsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<TeamDto>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
            {
                return Maybe<TeamDto>.None;
            }
            var teams = await _unitOfWork.TeamRepository.GetAll(cancellationToken);
            var countries = _unitOfWork.CountryRepository.Get();

            if (teams == null)
            {
                return Maybe<TeamDto>.None;
            }

            return new TeamDto(teams
                .Skip((request.PageNumber-1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new TeamItemDto(x.Id, 
                                                x.Name,
                                                x.ShortCode,
                                                x.CountryId, 
                                                countries?.FirstOrDefault(y=>y.Id==x.CountryId)?.Name!))
                .ToList());

        }
    }
}
