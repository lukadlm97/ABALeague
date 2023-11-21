using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Roster;
using OpenData.Basketball.AbaLeague.Domain.Common;

namespace OpenData.Basketball.AbaLeague.Application.Features.Rosters.Queries.GetAvailableRosterHistoryByTeamId
{
    public class GetAvailableRosterHistoryByTeamIdQueryHandler :  IQueryHandler<GetAvailableRosterHistoryByTeamIdQuery, Maybe<AvailableRostersDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAvailableRosterHistoryByTeamIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public Task<Maybe<AvailableRostersDto>> Handle(GetAvailableRosterHistoryByTeamIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
