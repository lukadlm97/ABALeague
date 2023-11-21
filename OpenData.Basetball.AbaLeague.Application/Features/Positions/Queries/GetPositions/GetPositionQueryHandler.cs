using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Position;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Positions.Queries.GetPositions
{
    internal class GetPositionQueryHandler : IQueryHandler<GetPositionQuery, Maybe<IEnumerable<PositionDto>>>
    {
        private IUnitOfWork _unitOfWork;

        public GetPositionQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Maybe<IEnumerable<PositionDto>>> Handle(GetPositionQuery request,
                                                            CancellationToken cancellationToken)
        {
            var positions = await _unitOfWork.PositionRepository.Get(cancellationToken);

            if(positions == null)
            {
                return Maybe<IEnumerable<PositionDto>>.None;
            }
            
            return positions.Select(x => new PositionDto(x.Id, x.Name)).ToList();
        }
    }
}
