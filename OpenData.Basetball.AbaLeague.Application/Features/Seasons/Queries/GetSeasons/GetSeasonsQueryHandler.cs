using OpenData.Basetball.AbaLeague.Application.Contracts;
using OpenData.Basketball.AbaLeague.Application.Abstractions.Messaging;
using OpenData.Basketball.AbaLeague.Application.DTOs.Season;
using OpenData.Basketball.AbaLeague.Domain.Common;
using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenData.Basketball.AbaLeague.Application.Features.Seasons.Queries.GetSeasons
{
    public class GetSeasonsQueryHandler : 
        IQueryHandler<GetSeasonsQuery, Maybe<SeasonDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSeasonsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Maybe<SeasonDto>> 
            Handle(GetSeasonsQuery request, CancellationToken cancellationToken)
        {
            return new SeasonDto(_unitOfWork.SeasonRepository.Get()
                                            .Select(x => new SeasonItemDto(x.Id, x.Name))
                                            .ToFrozenSet());
        }
    }
}
