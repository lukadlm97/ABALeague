using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.CompetionOrganization;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.CompetitionOrganization.Queries.GetCompetionOrganizations
{
    public class GetCompetionOrganizationsQueryHandler :
        IQueryHandler<GetCompetionOrganizationsQuery, Maybe<CompetionOrganizationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompetionOrganizationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<CompetionOrganizationDto>> 
            Handle(GetCompetionOrganizationsQuery request, CancellationToken cancellationToken)
        {
            return new CompetionOrganizationDto(_unitOfWork.CompetitionOrganizationRepository.Get()
                .Select(x => new CompetionOrganizationItemDto(x.Id, x.Name))
                .ToFrozenSet());
        }
    }
}
