using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.Features.SeasonResources.Commands.CreateSeasonResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenData.Basketball.AbaLeague.Application.DTOs.Team;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Teams.Queries.GetTeamById
{
    internal class GetTeamByIdQueryHandler : IQueryHandler<GetTeamByIdQuery, Maybe<TeamItemDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTeamByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<TeamItemDto>> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var existingTeam = await _unitOfWork.TeamRepository.Get(request.TeamId, cancellationToken);
                var countries = _unitOfWork.CountryRepository.Get();

                if (existingTeam == null)
                {
                    return Maybe<TeamItemDto>.None;
                }

                return new TeamItemDto(existingTeam.Id,
                                        existingTeam.Name, 
                                        existingTeam.ShortCode,
                                        existingTeam.CountryId,
                                        countries?.FirstOrDefault(x=>x.Id == existingTeam.CountryId)?.Name!);
            }
            catch (Exception e)
            {
                return Maybe<TeamItemDto>.None;
            }
        }
    }
}
